using System;
using System.IO;
using System.Collections.Generic;
/*
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
*/
using System.Windows.Forms;
using Newtonsoft.Json;
using DiscBotJSONManager;
using Google.Apis.Auth.OAuth2;
/* 
 * using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Docs.v1;
using Google.Apis.Docs.v1.Data;
*/
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.Threading;
using System.Diagnostics;
using System.Net;
using System.Linq;
using System.Security.Principal;
using System.Security.Cryptography;
using System.Text;


namespace MyDiscBotConfig
{


    public partial class MyDiscBotConfigForm : Form
    {
        private DiscBotConfig config = new DiscBotConfig();
        Process DiscBotProcess = new Process();
        delegate void DiscBotProcessDelegate(String msg);
        private Queue<string> _files = new Queue<string>();
        private bool discBotRunning = false;
        private string[] args;

        public MyDiscBotConfigForm()
        {
            args = Environment.GetCommandLineArgs();
            if (config.GameDayMap == null)
            {
                config.GameDayMap = new Dictionary<string, GameDayMap>()
                {
                    {  "0", new GameDayMap { Label="Day 7 DD KE", Profile="default"} },
                    {  "1", new GameDayMap { Label="Day 1 Gather", Profile="default"} },
                    {  "2", new GameDayMap { Label="Day 2 Build", Profile="default"} },
                    {  "3", new GameDayMap { Label="Day 3 Research", Profile="default"} },
                    {  "4", new GameDayMap { Label="Day 4 Hero", Profile="default"} },
                    {  "5", new GameDayMap { Label="Day 5 Training", Profile="default"} },
                    {  "6", new GameDayMap { Label="Day 6 KE", Profile="default"} }
                };
            }
            if ( config.cloudLogs == null )
            {
                config.cloudLogs = new cloudLogConfigEntry();
            }

            InitializeComponent();
            BotConfig.Text = Directory.GetCurrentDirectory().Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + "/mybot.json";
            populateFormFromConfig();
            screenshotToggle.Checked = true;
            MEMUInstances.Text = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile).Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + Path.AltDirectorySeparatorChar + ".MemuHyperv/MemuHyperv.xml";
            token.Enabled = false;
            Channel.Enabled = false;
            GlobalChannel.Enabled = false;
            announceStatus.Enabled = false;
            postStatusScreenshots.Enabled = false;
            ownerID.Enabled = false;
            ownerHandle.Enabled = false;
            Quip.Enabled = false;
            Status.Enabled = false;
            Announce.Enabled = false;
            Announcement.Enabled = false;
            if (File.Exists(DiscBotExePath.Text))
            {
                DiscBotExePath.Text = Path.GetFullPath(DiscBotExePath.Text).Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                if (IsAdmin() && File.Exists(BotConfig.Text))
                {
                    StartDiscBot.Enabled = true;
                    StartDiscBot.Text = "Start";
                    LoadPointer.Visible = false;
                    LauncherPathPointer.Visible = false;
                    loadBotConfig();
                }
            } else
            {
                StartDiscBot.Enabled = false;
                LoadPointer.Visible = true;
                LauncherPathPointer.Visible = true;
            }
        }

        private bool IsAdmin()
        {
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            };
        }

        private void loadBotConfig()
        {
            config = DiscBotConfig.FromJson(File.ReadAllText(BotConfig.Text));
            // new configurations won't have this
            if ( config.GameDayMap == null )
            {
                config.GameDayMap = new Dictionary<string, GameDayMap>() 
                {
                    {  "0", new GameDayMap { Label="Day 7 DD KE", Profile="default"} },
                    {  "1", new GameDayMap { Label="Day 1 Gather", Profile="default"} },
                    {  "2", new GameDayMap { Label="Day 2 Build", Profile="default"} },
                    {  "3", new GameDayMap { Label="Day 3 Research", Profile="default"} },
                    {  "4", new GameDayMap { Label="Day 4 Hero", Profile="default"} },
                    {  "5", new GameDayMap { Label="Day 5 Training", Profile="default"} },
                    {  "6", new GameDayMap { Label="Day 6 KE", Profile="default"} }
                };
            }
            if ( config.cloudLogs == null )
            {
                config.cloudLogs = new cloudLogConfigEntry();
            }
            populateFormFromConfig();
        }

        private void populateFormFromConfig() {
            DebugLevel.Value = config.Debug;
            if ( DebugLevel.Value > 0 )
            {
                debugEnableCheckbox.Checked = true;
            } else
            {
                debugEnableCheckbox.Checked = false;
            }
            GNBotDir.Text = config.GnBotDir;
            Launcher.Text = config.Launcher;
            LauncherFullPath.Text = config.GnBotDir + config.Launcher;
            GNBotLogDir.Text = Path.GetDirectoryName(GNBotDir.Text).Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + Path.AltDirectorySeparatorChar + "logs/";
            if (File.Exists(LauncherFullPath.Text))
            {
                Launcher.BackColor = System.Drawing.Color.LightGreen;
            }
            else
            {
                Launcher.BackColor = System.Drawing.Color.MistyRose;
            }
            GNBotSettings.Text = config.GnBotSettings;
            GNBotProfile.Text = config.GnBotProfile;
            MEMUPath.Text = config.MemuPath;
            MEMUC.Text = config.Memuc;
            MEMUInstances.Text = config.MemuInstances;
            BackupDir.Text = config.BackupDir;
            DuplicateLog.Text = config.DuplicateLog;
            DuplicateLogDirectory.Text = getDirectory(config.DuplicateLog);
            screenshotDir.Text = config.ScreenshotDir;
            if (config.Screenshot > 0)
            {
                screenshotToggle.Checked = true;
                if (config.PostStatusScreenshots > 0)
                {
                    postStatusScreenshots.Checked = true;
                }
            }
            else
            {
                screenshotToggle.Checked = false;
                postStatusScreenshots.Checked = false;
                config.PostStatusScreenshots = 0;
            }
            ConfigsDir.Text = config.ConfigsDir;
            gatherCSV.Text = config.GatherCsv;
            if (gatherCSV.Text == "")
            {
                EnableGatherCSV.Checked = false;
            } else
            {
                EnableGatherCSV.Checked = true;
            }
            nircmd.Text = config.Nircmd;
            ffmpeg.Text = config.Ffmpeg;
            if ( config.Token != "" )
            {
                useDiscord.Checked = true;
                token.Text = config.Token;
            }
            ownerID.Text = config.OwnerId;
            ownerHandle.Text = config.OwnerHandle;
            Status.Text = config.Status;
            Announcement.Text = config.Announcement;
            if (config.Announce > 0)
            {
                Announce.Checked = true;
            }
            else
            {
                Announce.Checked = false;
            }
            announceStatus.Value = config.AnnounceStatus;
            minimumCycleTime.Value = config.MinimumCycleTime;
            GNBotThreads.Value = config.GnBotThreads;
            Channel.Text = config.Channel;
            GlobalChannel.Text = config.GlobalChannel;
            GoogleSpreadsheetID.Text = config.GoogleSheetID;
            GoogleCredentialsFilePath.Text = config.GoogleSecretsFile;
            GoogleTokenFilePath.Text = config.GoogleTokenFile;
            GoogleSpreadsheetWorksheetName.Text = config.GoogleWorksheetName;
            GoogleAppName.Text = config.GoogleAppName;
            GoogleButton.Enabled = false;
            if ( config.UseGoogle > 0 )
            {
                config.UseGoogle = 1;
                useGoogle.Checked = true;
            } else
            {
                config.UseGoogle = 0;
                useGoogle.Checked = false;
            }
            Quip.Text = config.Quip;
            if (config.enableReboot > 0)
            {
                EnableReboot.Checked = true;
            } else
            {
                EnableReboot.Checked = false;
            }
            if ( config.GNBotRestartFullCycle > 0 )
            {
                GNBotRestartFullCycle.Checked = true;
                GNBotRestartInterval.Enabled = false;
            } else
            {
                GNBotRestartFullCycle.Checked = false;
                GNBotRestartInterval.Enabled = true;
                GNBotRestartInterval.Value = config.GNBotRestartInterval;
            }
            GNBotWindowX.Value = config.MoveGNBotWindow[0];
            GNBotWindowY.Value = config.MoveGNBotWindow[1];
            GNBotWindowWidth.Value = config.MoveGNBotWindow[2];
            GNBotWindowHeight.Value = config.MoveGNBotWindow[3];
            Day1Label.Text = config.GameDayMap["1"].Label;
            Day2Label.Text = config.GameDayMap["2"].Label;
            Day3Label.Text = config.GameDayMap["3"].Label;
            Day4Label.Text = config.GameDayMap["4"].Label;
            Day5Label.Text = config.GameDayMap["5"].Label;
            Day6Label.Text = config.GameDayMap["6"].Label;
            Day0Label.Text = config.GameDayMap["0"].Label;
            Day1Profile.Text = config.GameDayMap["1"].Profile;
            Day2Profile.Text = config.GameDayMap["2"].Profile;
            Day3Profile.Text = config.GameDayMap["3"].Profile;
            Day4Profile.Text = config.GameDayMap["4"].Profile;
            Day5Profile.Text = config.GameDayMap["5"].Profile;
            Day6Profile.Text = config.GameDayMap["6"].Profile;
            Day0Profile.Text = config.GameDayMap["0"].Profile;
            ActivateBaseTime.Value = config.ActiveBaseTimer;
            MaximumFailures.Value = config.MaxFailures;
            PausedMasterFile.Text = config.PausedMaster;
            if ( config.enableReboot > 0 )
            {
                EnableReboot.Checked = true;
            } else
            {
                EnableReboot.Checked = false;
            }
            if ( config.cloudLogs.Enabled > 0 )
            {
                CloudLogEnable.Checked = true;
                CloudLogEndpoint.Text = config.cloudLogs.Endpoint;
                CloudLogHost.Text = config.cloudLogs.Host;
                CloudLogPort.Value = config.cloudLogs.Port;
                CloudLogSource.Text = config.cloudLogs.Source;
                CloudLogToken.Text = config.cloudLogs.Token;
                CloudLogSubmitFunction.Text = config.cloudLogs.Submit;
                CloudLogInitFunction.Text = config.cloudLogs.Init;
            } else
            {
                CloudLogEnable.Checked = false;
            }
        }

        void LaunchDiscBotProcess(string DiscBotExe = "MyBot-Win.exe")
        {
            DiscBotProcess.EnableRaisingEvents = true;
            DiscBotProcess.OutputDataReceived += new System.Diagnostics.DataReceivedEventHandler(DiscBotProcess_OutputDataReceived);
            DiscBotProcess.ErrorDataReceived += new System.Diagnostics.DataReceivedEventHandler(DiscBotProcess_ErrorDataReceived);
            DiscBotProcess.Exited += new System.EventHandler(DiscBotProcess_Exited);

            DiscBotProcess.StartInfo.FileName = DiscBotExe;
            DiscBotProcess.StartInfo.Arguments = null;
            DiscBotProcess.StartInfo.UseShellExecute = false;
            DiscBotProcess.StartInfo.Verb = "runas";
            DiscBotProcess.StartInfo.RedirectStandardError = true;
            DiscBotProcess.StartInfo.RedirectStandardOutput = true;
            if (ShowConsole.Checked == true)
            {
                DiscBotProcess.StartInfo.RedirectStandardError = false;
                DiscBotProcess.StartInfo.CreateNoWindow = false;
            } else
            {
                DiscBotProcess.StartInfo.CreateNoWindow = true;
            }
            DiscBotProcess.StartInfo.WorkingDirectory = getDirectory(DiscBotExe);

            discBotRunning = DiscBotProcess.Start();
            try
            {
                DiscBotProcess.BeginErrorReadLine();
            }
            //            catch (InvalidOperationException e) when (e.Message.Contains("Async"))
            //            catch (InvalidOperationException e)
            catch           {
                //                DiscBotProcess.CancelErrorRead();
                //                DiscBotProcess.BeginErrorReadLine();
            }
            try
            {
                DiscBotProcess.BeginOutputReadLine();
            }
            catch
            {
                //                DiscBotProcess.CancelOutputRead();
                //                DiscBotProcess.BeginOutputReadLine();
            }
            //uncomment to make this a blocking call that waits until the bot process is closed
            //process.WaitForExit();
        }

        private void DiscBotProcessOutput(String message = "")
        {
            // Check whether the caller must call an invoke method when making method calls to listBoxCCNetOutput because the caller is 
            // on a different thread than the one the listBoxCCNetOutput control was created on.
            if (DiscBotOutput.InvokeRequired)
            {
                DiscBotProcessDelegate update = new DiscBotProcessDelegate(DiscBotProcessOutput);
                DiscBotOutput.Invoke(update, message);
            }
            else
            {
                if (message == null) { message = ""; }
                DiscBotOutput.Items.Add(message);
                if (DiscBotOutput.Items.Count > numericUpDown1.Value)
                { // If the buffer is shrunk, clear the entries
                    for (int x = (DiscBotOutput.Items.Count - (int)numericUpDown1.Value); x >= 0; x--)
                    {
                        DiscBotOutput.Items.RemoveAt(x);
                    }
                }
                DiscBotOutput.SelectedIndex = DiscBotOutput.Items.Count - 1;
                DiscBotOutput.ClearSelected();
            }
        }

        void DiscBotProcess_Exited(object sender, EventArgs e)
        {
            try
            {
                // race condition exists for some reason. The process can close before we get here and the reference won't exist
                DiscBotProcessOutput(string.Format("process exited with code {0}\n", DiscBotProcess.ExitCode.ToString()));
            } catch
            {
                // finish the conditions we care to handle
            }
            discBotRunning = false;

        }

        void DiscBotProcess_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            try
            {
                DiscBotProcessOutput(e.Data);
            } catch
            {

            }
        }

        void DiscBotProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            try
            {
                DiscBotProcessOutput(e.Data);
            } catch
            {

            }
        }

        private void label6_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "GNLauncher.exe";
            if ( openFileDialog1.ShowDialog() == DialogResult.OK )
            {
                LauncherFullPath.Text = openFileDialog1.FileName.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                GNBotDir.Text = Path.GetDirectoryName(LauncherFullPath.Text).Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + Path.AltDirectorySeparatorChar;
                // Launcher.Text = "";
                Launcher.Text = Path.GetFileName(LauncherFullPath.Text);
                GNBotProfile.Text = GNBotDir.Text + "profiles/actions/LSSBot/default.json";
                GNBotSettings.Text = GNBotDir.Text + "settings.json";
                GNBotLogDir.Text = GNBotDir.Text + "Logs/";
            }
        }

        private void GNBotSettings_TextChanged(object sender, EventArgs e)
        {
            config.GnBotSettings = GNBotSettings.Text;
            if ( File.Exists(GNBotSettings.Text))
            {
                GNBotSettings.BackColor = System.Drawing.Color.LightGreen;
            }
            else
            {
                GNBotSettings.BackColor = System.Drawing.Color.MistyRose;
            }
        }

        private void label16_Click(object sender, EventArgs e)
        {
            if ( folderBrowserDialog1.ShowDialog()== DialogResult.OK)
            {
                DuplicateLogDirectory.Text = folderBrowserDialog1.SelectedPath.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + Path.AltDirectorySeparatorChar;
                DuplicateLog.Text = DuplicateLogDirectory.Text + "LssSessions.log";
            }
        }

        private void Launcher_TextChanged(object sender, EventArgs e)
        {
            config.Launcher = Launcher.Text;
            LauncherFullPath.Text = GNBotDir.Text + Launcher.Text;
            if ( File.Exists(LauncherFullPath.Text))
            {
                Launcher.BackColor = System.Drawing.Color.LightGreen;
            }
            else
            {
                Launcher.BackColor = System.Drawing.Color.MistyRose;
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label19_Click(object sender, EventArgs e)
        {
            if ( folderBrowserDialog1.ShowDialog() == DialogResult.OK )
            {
                MEMUPath.Text = folderBrowserDialog1.SelectedPath.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + Path.AltDirectorySeparatorChar;
            }
        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void MEMUPath_TextChanged(object sender, EventArgs e)
        {
            config.MemuPath = MEMUPath.Text;
            if ( Directory.Exists(MEMUPath.Text))
            {
                MEMUPath.BackColor = System.Drawing.Color.LightGreen;
            }
            else
            {
                MEMUPath.BackColor = System.Drawing.Color.MistyRose;
            }
        }

        private void label22_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "memuc.exe";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                MEMUC.Text = openFileDialog1.FileName.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                MEMUPath.Text = Path.GetDirectoryName(MEMUC.Text).Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + Path.AltDirectorySeparatorChar + "MemuHyperv VMs";
                MEMUInstances.Text = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile).Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + Path.AltDirectorySeparatorChar + ".MemuHyperv/MemuHyperv.xml";
            }
        }

        private void label24_Click(object sender, EventArgs e)
        {
            if ( openFileDialog1.ShowDialog() == DialogResult.OK )
            {
                gatherCSV.Text = openFileDialog1.FileName.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            }
        }

        private void label26_Click(object sender, EventArgs e)
        {
            if ( folderBrowserDialog1.ShowDialog() == DialogResult.OK )
            {
                BackupDir.Text = folderBrowserDialog1.SelectedPath.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + Path.AltDirectorySeparatorChar;
            }
        }

        private void label28_Click(object sender, EventArgs e)
        {
            if ( folderBrowserDialog1.ShowDialog() == DialogResult.OK )
            {
                ConfigsDir.Text = folderBrowserDialog1.SelectedPath.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + Path.AltDirectorySeparatorChar;
            }
        }

        private void screenshotToggle_CheckedChanged(object sender, EventArgs e)
        {
            if ( screenshotToggle.Checked == true)
            {
                config.Screenshot = 1;
                screenshotDir.Enabled = true;
                postStatusScreenshots.Enabled = true;
                ScreenshotDirSelector.Enabled = true;
                screenshotDir.Text = config.ScreenshotDir;
            }
            else
            {
                config.Screenshot = 0;
                screenshotDir.Enabled = false;
                postStatusScreenshots.Checked = false;
                postStatusScreenshots.Enabled = false;
                ScreenshotDirSelector.Enabled = false;
//                screenshotDir.Text = "DISABLED";
            }
        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label32_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "nircmd.exe";
            if ( openFileDialog1.ShowDialog() == DialogResult.OK )
            {
                nircmd.Text = openFileDialog1.FileName.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            }
        }

        private void label34_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "ffmpeg.exe";
            if ( openFileDialog1.ShowDialog() == DialogResult.OK )
            {
                ffmpeg.Text = openFileDialog1.FileName.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            }
        }

        private void label36_Click(object sender, EventArgs e)
        {

        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            BotConfig.Text = FilePicker("Config File | *.json");
            if ( File.Exists(BotConfig.Text) )
            {
                loadBotConfig();
                DiscBotExePath.Text = getDirectory(BotConfig.Text) + getFileName(DiscBotExePath.Text);
                if (File.Exists(DiscBotExePath.Text) && IsAdmin())
                {
                    StartDiscBot.Enabled = true;
                    StartDiscBot.Text = "Start";
                } else
                {
                    StartDiscBot.Text = "Admin?";
                }
                LoadPointer.Visible = false;
                LauncherPathPointer.Visible = false;
                SaveResult.Text = "Loaded config from " + BotConfig.Text;
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            File.WriteAllText(BotConfig.Text, Serialize.ToJson(config));
            SaveResult.Text = "Saved config to " + BotConfig.Text;
        }

        private void label44_Click(object sender, EventArgs e)
        {
            if ( folderBrowserDialog1.ShowDialog() == DialogResult.OK )
            {
                GNBotLogDir.Text = folderBrowserDialog1.SelectedPath.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + "/";
                GNBotLogMask.Text = GNBotLogDir.Text + LogFileNameMask.Text;
                GNBotLogMain.Text = GNBotLogDir.Text + MainLogName.Text;
            }
        }

        private void GNBotLogDir_TextChanged(object sender, EventArgs e)
        {
            GNBotLogMask.Text = GNBotLogDir.Text + LogFileNameMask.Text;
            GNBotLogMain.Text = GNBotLogDir.Text + MainLogName.Text;
            if ( Directory.Exists(GNBotLogDir.Text))
            {
                GNBotLogDir.BackColor = System.Drawing.Color.LightGreen;
            }
            else
            {
                GNBotLogDir.BackColor = System.Drawing.Color.MistyRose;
            }
        }

        private void DuplicateLog_TextChanged(object sender, EventArgs e)
        {
            config.DuplicateLog = DuplicateLog.Text;
            DuplicateLogDirectory.Text = Path.GetDirectoryName(DuplicateLog.Text).Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + Path.AltDirectorySeparatorChar;
            if ( Directory.Exists(DuplicateLogDirectory.Text))
            {
                DuplicateLog.BackColor = System.Drawing.Color.LightGreen;
            }
            else
            {
                DuplicateLog.BackColor = System.Drawing.Color.MistyRose;
            }
        }

        private void GNBotLogMain_TextChanged(object sender, EventArgs e)
        {
            config.GnBotLogMain = GNBotLogMain.Text;
            if ( File.Exists(GNBotLogMain.Text))
            {
                GNBotLogMain.BackColor = System.Drawing.Color.LightGreen;
            }
            else
            {
                GNBotLogMain.BackColor = System.Drawing.Color.MistyRose;
            }
        }

        private void GNBotLogMask_TextChanged(object sender, EventArgs e)
        {
            config.GnBotLogMask = GNBotLogMask.Text;
            if ( File.Exists(GNBotLogDir.Text + "log_1.txt"))
            {
                GNBotLogMask.BackColor = System.Drawing.Color.LightGreen;
            }
            else
            {
                GNBotLogMask.BackColor = System.Drawing.Color.MistyRose;
            }
        }

        private void Announce_CheckedChanged(object sender, EventArgs e)
        {
            if ( Announce.Checked == true )
            {
                config.Announce = 1;
                Announcement.Text = config.Announcement;
                Announcement.ReadOnly = false;
                Announcement.BorderStyle = BorderStyle.Fixed3D;
            } else
            {
                config.Announce = 0;
                Announcement.Text = "DISABLED";
                Announcement.ReadOnly = true;
                Announcement.BorderStyle = BorderStyle.None;
            }
        }

        private void LauncherFullPath_TextChanged(object sender, EventArgs e)
        {
            if ( File.Exists(LauncherFullPath.Text))
            {
                LauncherFullPath.BackColor = System.Drawing.Color.LightGreen;
            } else
            {
                LauncherFullPath.BackColor = System.Drawing.Color.MistyRose;
            }
        }

        private void GNBotDir_TextChanged(object sender, EventArgs e)
        {
            config.GnBotDir = GNBotDir.Text;
            if ( Directory.Exists(GNBotDir.Text))
            {
                GNBotDir.BackColor = System.Drawing.Color.LightGreen;
            }
            else
            {
                GNBotDir.BackColor = System.Drawing.Color.MistyRose;
            }
        }

        private void GNBotProfile_TextChanged(object sender, EventArgs e)
        {
            config.GnBotProfile = GNBotProfile.Text;
            if ( File.Exists(GNBotProfile.Text))
            {
                GNBotProfile.BackColor = System.Drawing.Color.LightGreen;
            }
            else
            {
                GNBotProfile.BackColor = System.Drawing.Color.MistyRose;
            }
        }

        private void DuplicateLogDirectory_TextChanged(object sender, EventArgs e)
        {
            if ( Directory.Exists(DuplicateLogDirectory.Text))
            {
                DuplicateLogDirectory.BackColor = System.Drawing.Color.LightGreen;
            }
            else
            {
                DuplicateLogDirectory.BackColor = System.Drawing.Color.MistyRose;
            }
        }

        private void MEMUC_TextChanged(object sender, EventArgs e)
        {
            config.Memuc = MEMUC.Text;
            if ( File.Exists(MEMUC.Text))
            {
                MEMUC.BackColor = System.Drawing.Color.LightGreen;
            }
            else
            {
                MEMUC.BackColor = System.Drawing.Color.MistyRose;
            }
        }

        private void gatherCSV_TextChanged(object sender, EventArgs e)
        {
            config.GatherCsv = gatherCSV.Text;
            if ( File.Exists(gatherCSV.Text))
            {
                gatherCSV.BackColor = System.Drawing.Color.LightGreen;
            }
            else
            {
                gatherCSV.BackColor = System.Drawing.Color.MistyRose;
            }
        }

        private void MEMUInstances_TextChanged(object sender, EventArgs e)
        {
            config.MemuInstances = MEMUInstances.Text;
            if ( File.Exists(MEMUInstances.Text))
            {
                MEMUInstances.BackColor = System.Drawing.Color.LightGreen;
            }
            else
            {
                MEMUInstances.BackColor = System.Drawing.Color.MistyRose;
            }
        }

        private void BackupDir_TextChanged(object sender, EventArgs e)
        {
            config.BackupDir = BackupDir.Text;
            if ( Directory.Exists(BackupDir.Text))
            {
                BackupDir.BackColor = System.Drawing.Color.LightGreen;
            }
            else
            {
                BackupDir.BackColor = System.Drawing.Color.MistyRose;
            }
        }

        private void ConfigsDir_TextChanged(object sender, EventArgs e)
        {
            config.ConfigsDir = ConfigsDir.Text;
            if ( Directory.Exists(ConfigsDir.Text))
            {
                ConfigsDir.BackColor = System.Drawing.Color.LightGreen;
            }
            else
            {
                ConfigsDir.BackColor = System.Drawing.Color.MistyRose;
            }
        }

        private void screenshotDir_TextChanged(object sender, EventArgs e)
        {
            config.ScreenshotDir = screenshotDir.Text;
            if ( Directory.Exists(screenshotDir.Text))
            {
                screenshotDir.BackColor = System.Drawing.Color.LightGreen;
            }
            else
            {
                screenshotDir.BackColor = System.Drawing.Color.MistyRose;
            }
        }

        private void nircmd_TextChanged(object sender, EventArgs e)
        {
            config.Nircmd = nircmd.Text;
            if ( File.Exists(nircmd.Text))
            {
                nircmd.BackColor = System.Drawing.Color.LightGreen;
            }
            else
            {
                nircmd.BackColor = System.Drawing.Color.MistyRose;
            }
        }

        private void ffmpeg_TextChanged(object sender, EventArgs e)
        {
            config.Ffmpeg = ffmpeg.Text;
            if ( File.Exists(ffmpeg.Text))
            {
                ffmpeg.BackColor = System.Drawing.Color.LightGreen;
            }
            else
            {
                ffmpeg.BackColor = System.Drawing.Color.MistyRose;
            }
        }

        private void token_TextChanged(object sender, EventArgs e)
        {
            config.Token = token.Text;
        }

        private void GNBotThreads_ValueChanged(object sender, EventArgs e)
        {
            config.GnBotThreads = (long)GNBotThreads.Value;
        }

        private void minimumCycleTime_ValueChanged(object sender, EventArgs e)
        {
            config.MinimumCycleTime = (long)minimumCycleTime.Value;
        }

        private void announceStatus_ValueChanged(object sender, EventArgs e)
        {
            config.AnnounceStatus = (long)announceStatus.Value;
        }

        private void postStatusScreenshots_CheckedChanged(object sender, EventArgs e)
        {
            if ( postStatusScreenshots.Checked == true)
            {
                config.PostStatusScreenshots = 1;
            } else
            {
                config.PostStatusScreenshots = 0;
            }
        }

        private void ownerID_TextChanged(object sender, EventArgs e)
        {
            config.OwnerId = ownerID.Text;
        }

        private void ownerHandle_TextChanged(object sender, EventArgs e)
        {
            config.OwnerHandle = ownerHandle.Text;
        }

        private void Channel_TextChanged(object sender, EventArgs e)
        {
            config.Channel = Channel.Text;
        }

        private void Quip_TextChanged(object sender, EventArgs e)
        {
            config.Quip = Quip.Text;
        }

        private void GlobalChannel_TextChanged(object sender, EventArgs e)
        {
            config.GlobalChannel = GlobalChannel.Text;
        }

        private void Status_TextChanged(object sender, EventArgs e)
        {
            config.Status = Status.Text;
        }

        private void Announcement_TextChanged(object sender, EventArgs e)
        {
            config.Announcement = Announcement.Text;
        }

        private void MyDiscBotConfigForm_Load(object sender, EventArgs e)
        {
            if (!IsAdmin())
            {
                StartDiscBot.Enabled = false;
                StartDiscBot.Text = "Admin?";
            }
            else
            {
                // running as admin, should we start?
                if (args.Contains("-start"))
                {
                    DiscBotOutput.Items.Add("Automatic start requested");
                    StartDiscBot.Enabled = true;
                    ConfigurationTabs.SelectedTab = RunDiscBotTab;
                    StartDiscBot.PerformClick();
                }
            }
        }

        private void label30_Click_1(object sender, EventArgs e)
        {
            Form TokenInstructionsForm = new TokenInstructions();
            // TokenInstructionsForm.TopLevel = false;
            this.AddOwnedForm(TokenInstructionsForm);
            TokenInstructionsForm.Show(this);
            TokenInstructionsForm.SetDesktopLocation(this.Location.X + 10, this.Location.Y + 10);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (useDiscord.Checked == true)
            {
                token.Enabled = true;
                Channel.Enabled = true;
                GlobalChannel.Enabled = true;
                announceStatus.Enabled = true;
                postStatusScreenshots.Enabled = true;
                ownerID.Enabled = true;
                ownerHandle.Enabled = true;
                Quip.Enabled = true;
                Status.Enabled = true;
                Announce.Enabled = true;
                Announcement.Enabled = true;
            } else
            {
                token.Text = "";
                token.Enabled = false;
                Channel.Enabled = false;
                GlobalChannel.Enabled = false;
                announceStatus.Enabled = false;
                postStatusScreenshots.Enabled = false;
                ownerID.Enabled = false;
                ownerHandle.Enabled = false;
                Quip.Enabled = false;
                Status.Enabled = false;
                Announce.Enabled = false;
                Announcement.Enabled = false;
            }
        }

        private void label47_Click(object sender, EventArgs e)
        {
            if (label47.Text == "***")
            {
                token.UseSystemPasswordChar = true;
                label47.Text = "ABC";
            }
            else
            {
                token.UseSystemPasswordChar = false;
                label47.Text = "***";
            }
        }

        private void label49_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                GoogleCredentialsFilePath.Text = openFileDialog1.FileName.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                // googleSecretsFile.Text = Path.GetDirectoryName(LauncherFullPath.Text).Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + Path.AltDirectorySeparatorChar;
                // Launcher.Text = "";
            }
        }

        private void useGoogle_CheckedChanged(object sender, EventArgs e)
        {
            if (useGoogle.Checked == true)
            {
                GoogleCredentialsFilePath.Enabled = true;
                GoogleTokenFilePath.Enabled = true;
                GoogleAppName.Enabled = true;
                GoogleSpreadsheetID.Enabled = true;
                GoogleSpreadsheetWorksheetName.Enabled = true;
                config.UseGoogle = 1;
            }
            else
            {
                GoogleCredentialsFilePath.Enabled = false;
                GoogleTokenFilePath.Enabled = false;
                GoogleAppName.Enabled = false;
                GoogleSpreadsheetID.Enabled = false;
                GoogleSpreadsheetWorksheetName.Enabled = false;
                config.UseGoogle = 0;
            }

        }

        private void GoogleCredentialsFilePath_TextChanged(object sender, EventArgs e)
        {
            if (File.Exists(GoogleCredentialsFilePath.Text))
            {
                config.GoogleSecretsFile = GoogleCredentialsFilePath.Text;
                GoogleCredentialsFilePath.BackColor = System.Drawing.Color.LightGreen;
                config.UseGoogle = 1;
                GoogleTokenFilePath.Text = Path.GetDirectoryName(GoogleCredentialsFilePath.Text).Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + Path.AltDirectorySeparatorChar + "token.json";
            }
            else
            {
                GoogleCredentialsFilePath.BackColor = System.Drawing.Color.MistyRose;
                config.UseGoogle = 0;
            }
        }

        private void label50_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                GoogleTokenFilePath.Text = openFileDialog1.FileName.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            }
        }

        private void GoogleTokenFilePath_TextChanged_1(object sender, EventArgs e)
        {
            config.GoogleTokenFile = GoogleTokenFilePath.Text;
            if (File.Exists(GoogleTokenFilePath.Text))
            {
                GoogleTokenFilePath.BackColor = System.Drawing.Color.LightGreen;
                GoogleButton.Text = "Test";
                GoogleButton.BackColor = System.Drawing.Color.MistyRose;
            }
            else
            {
                GoogleTokenFilePath.BackColor = System.Drawing.Color.White;
                GoogleButton.Text = "Generate";
                GoogleButton.BackColor = System.Drawing.Color.MistyRose;
            }
        }

        private void label46_Click(object sender, EventArgs e)
        {

        }

        private void label45_Click(object sender, EventArgs e)
        {
            if (label45.Text == "***")
            {
                GoogleSpreadsheetID.UseSystemPasswordChar = true;
                label45.Text = "ABC";
            }
            else
            {
                GoogleSpreadsheetID.UseSystemPasswordChar = false;
                label45.Text = "***";
            }
        }

        private void GoogleButton_Click(object sender, EventArgs e)
        {
            string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
//            string AppName = "quickstart-1583290575904";
            string AppName = GoogleAppName.Text;

            UserCredential credential;

            var secrets = GoogleClientSecrets.Load(new FileStream(GoogleCredentialsFilePath.Text, FileMode.Open, FileAccess.Read));

            CancellationTokenSource cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(60));
            CancellationToken ct = cts.Token;

            string storedCredPath = "MyDiscordBot.json";
            credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                secrets.Secrets,
                Scopes,
                "user",
                ct,
                new FileDataStore(storedCredPath, false)).Result;
            
            if (ct.IsCancellationRequested) return;

            // Save the token into a file for use by the bot
            File.WriteAllText(GoogleTokenFilePath.Text, JsonConvert.SerializeObject(credential.Token));
            if (GoogleButton.Text == "Generate")
            {
                GoogleButton.Text = "Test";
                GoogleTokenFilePath.BackColor = System.Drawing.Color.LightGreen;
                GoogleButton.BackColor = DefaultBackColor;
            }
            // Create Google Sheets API service.
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = AppName,
            });

            // Define request parameters.
            String spreadsheetId = GoogleSpreadsheetID.Text;
            String range = GoogleSpreadsheetWorksheetName.Text;
            String CSVText = "";
            SpreadsheetsResource.ValuesResource.GetRequest request =
                    service.Spreadsheets.Values.Get(spreadsheetId, range);

            ValueRange response = request.Execute();
            IList<IList<Object>> values = response.Values;
            if (values != null && values.Count > 0)
            {
                foreach (var row in values)
                {
                    // XXX - Todo: Make this dynamic and consider using a listbox control instead... 
                    CSVText = CSVText + row[0] + "," + row[1] + "," + row[2] + "," + row[3] + "," + row[4] + row[5] + "," + row[6] + "," + row[7] + "," + row[8] + "," + row[9] + "," + row[10] + "," + row[11] + "," + row[12] + "," + row[13] + "," + row[14] + "," + row[15] + "," + row[16] + "," + row[17] + "\r\n";
                }
            }
            else
            {
                CSVText = "No data found.";
            }
            Output.Text = CSVText;          
        }

        private void GoogleSpreadsheetID_TextChanged(object sender, EventArgs e)
        {
            config.GoogleSheetID = GoogleSpreadsheetID.Text;
        }

        private void GoogleAppName_TextChanged(object sender, EventArgs e)
        {
            config.GoogleAppName = GoogleAppName.Text;
        }

        private void GoogleSpreadsheetWorksheetName_TextChanged(object sender, EventArgs e)
        {
            config.GoogleWorksheetName = GoogleSpreadsheetWorksheetName.Text;
        }

        private void EnableReboot_CheckedChanged(object sender, EventArgs e)
        {
            if (EnableReboot.Checked == true)
            {
                config.enableReboot = 1;
            } 
            else 
            { 
                config.enableReboot = 0;
            }
        }

        private void label55_Click_1(object sender, EventArgs e)
        {

        }

        private void GNBotRestartFullCycle_CheckedChanged(object sender, EventArgs e)
        {
            if ( GNBotRestartFullCycle.Checked == true )
            {
                config.GNBotRestartFullCycle = 1;
                config.GNBotRestartInterval = 0;
                GNBotRestartInterval.Enabled = false;
            } else
            {
                config.GNBotRestartFullCycle = 0;
                GNBotRestartInterval.Enabled = true;
            }
        }

        private void GNBotRestartInterval_ValueChanged(object sender, EventArgs e)
        {
            config.GNBotRestartInterval = (long)GNBotRestartInterval.Value;
        }

        private void GoogleAppName_TextChanged_1(object sender, EventArgs e)
        {
            config.GoogleAppName = GoogleAppName.Text;
        }

        private void label52_Click(object sender, EventArgs e)
        {

        }

        private void GoogleSpreadsheetWorksheetName_TextChanged_1(object sender, EventArgs e)
        {
            config.GoogleWorksheetName = GoogleSpreadsheetWorksheetName.Text;
        }

        private void label53_Click(object sender, EventArgs e)
        {

        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void useDiscord_CheckedChanged(object sender, EventArgs e)
        {
            if (useDiscord.Checked == true )
            {
                token.Enabled = true;
                Channel.Enabled = true;
                GlobalChannel.Enabled = true;
                ownerID.Enabled = true;
                Quip.Enabled = true;
                Status.Enabled = true;
                ownerHandle.Enabled = true;
                Announce.Enabled = true;
                Announcement.Enabled = true;
                announceStatus.Enabled = true;
                if (screenshotToggle.Checked)
                {
                    postStatusScreenshots.Enabled = true;
                }
            }
            else
            {
                token.Enabled = false;
                Channel.Enabled = false;
                GlobalChannel.Enabled = false;
                ownerID.Enabled = false;
                Quip.Enabled = false;
                Status.Enabled = false;
                ownerHandle.Enabled = false;
                Announce.Enabled = false;
                Announcement.Enabled = false;
                announceStatus.Enabled = false;
                postStatusScreenshots.Enabled = false;
            }
        }

        private void GNLauncherPicker_Click(object sender, EventArgs e)
        {
            LauncherFullPath.Text = FilePicker("Executable | *.exe");
            GNBotDir.Text = getDirectory(LauncherFullPath.Text);
            Launcher.Text = getFileName(LauncherFullPath.Text);
            GNBotProfile.Text = GNBotDir.Text + "profiles/actions/LSSBot/default.json";
            GNBotSettings.Text = GNBotDir.Text + "settings.json";
            GNBotLogDir.Text = GNBotDir.Text + "Logs/";
        }

        private string FilePicker(string fileMask = "All Files | *.*")
        {
            openFileDialog1.Filter = fileMask;
            openFileDialog1.FileName = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                return openFileDialog1.FileName.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            } else
            {
                return "Not found. Please select the appropriate file.";
            }
        }

        private string DirectoryPicker()
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                return folderBrowserDialog1.SelectedPath.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + Path.AltDirectorySeparatorChar;
            } else
            {
                return null;
            }
        }

        private string getDirectory(string directoryPath)
        {
            return Path.GetDirectoryName(Path.GetFullPath(directoryPath)).Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + Path.AltDirectorySeparatorChar;
        }

        private string getFileName(string filePath)
        {
            return Path.GetFileName(filePath);
        }

        private void label47_Click_1(object sender, EventArgs e)
        {
            if (label47.Text == "***")
            {
                token.UseSystemPasswordChar = true;
                label47.Text = "ABC";
            }
            else
            {
                token.UseSystemPasswordChar = false;
                label47.Text = "***";
            }
        }

        private void screenshotToggle_CheckedChanged_1(object sender, EventArgs e)
        {
            if ( screenshotToggle.Checked )
            {
                config.Screenshot = 1;
                screenshotDir.Enabled = true;
                postStatusScreenshots.Enabled = true;
                nircmd.Enabled = true;
                ffmpeg.Enabled = true;
            } else
            {
                config.Screenshot = 0;
                screenshotDir.Enabled = false;
                postStatusScreenshots.Enabled = false;
                nircmd.Enabled = false;
                ffmpeg.Enabled = false;
            }
        }

        private void token_TextChanged_1(object sender, EventArgs e)
        {
            config.Token = token.Text;
        }

        private void LauncherFullPath_TextChanged_1(object sender, EventArgs e)
        {
            config.Launcher = Launcher.Text;
            if (File.Exists(LauncherFullPath.Text))
            {
                LauncherFullPath.BackColor = System.Drawing.Color.LightGreen;
            }
            else
            {
                LauncherFullPath.BackColor = System.Drawing.Color.MistyRose;
            }
        }

        private void GNBotDir_TextChanged_1(object sender, EventArgs e)
        {
            config.GnBotDir = GNBotDir.Text;
            if ( Directory.Exists(GNBotDir.Text) )
            {
                GNBotDir.BackColor = System.Drawing.Color.LightGreen;
            } else
            {
                GNBotDir.BackColor = System.Drawing.Color.MistyRose;
            }
        }

        private void Launcher_TextChanged_1(object sender, EventArgs e)
        {
            config.Launcher = Launcher.Text;
            if (File.Exists(LauncherFullPath.Text))
            {
                Launcher.BackColor = System.Drawing.Color.LightGreen;
            }
            else
            {
                Launcher.BackColor = System.Drawing.Color.MistyRose;
            }

        }

        private void GNBotThreads_ValueChanged_1(object sender, EventArgs e)
        {
            config.GnBotThreads = (long)GNBotThreads.Value;
        }

        private void GNBotProfile_TextChanged_1(object sender, EventArgs e)
        {
            config.GnBotProfile = GNBotProfile.Text;
            if (File.Exists(GNBotProfile.Text))
            {
                GNBotProfile.BackColor = System.Drawing.Color.LightGreen;
            }
            else
            {
                GNBotProfile.BackColor = System.Drawing.Color.MistyRose;
            }
        }

        private void GNBotLogDir_TextChanged_1(object sender, EventArgs e)
        {
            GNBotLogMask.Text = GNBotLogDir.Text + LogFileNameMask.Text;
            config.GnBotLogMask = GNBotLogMask.Text;
            GNBotLogMain.Text = GNBotLogDir.Text + MainLogName.Text;
            config.GnBotLogMain = GNBotLogMain.Text;
            if (Directory.Exists(GNBotLogDir.Text))
            {
                GNBotLogDir.BackColor = System.Drawing.Color.LightGreen;
            }
            else
            {
                GNBotLogDir.BackColor = System.Drawing.Color.MistyRose;
            }
        }

        private void DuplicateLogDirectory_TextChanged_1(object sender, EventArgs e)
        {
            config.DuplicateLog = DuplicateLogDirectory.Text + Path.AltDirectorySeparatorChar + getFileName(DuplicateLog.Text);
            if (Directory.Exists(DuplicateLogDirectory.Text))
            {
                DuplicateLogDirectory.BackColor = System.Drawing.Color.LightGreen;
            }
            else
            {
                DuplicateLogDirectory.BackColor = System.Drawing.Color.MistyRose;
            }
        }

        private void label16_Click_1(object sender, EventArgs e)
        {
            DuplicateLogDirectory.Text = DirectoryPicker();
        }

        private void MEMUC_TextChanged_1(object sender, EventArgs e)
        {
            config.Memuc = MEMUC.Text;
            if (File.Exists(MEMUC.Text))
            {
                MEMUC.BackColor = System.Drawing.Color.LightGreen;
            }
            else
            {
                MEMUC.BackColor = System.Drawing.Color.MistyRose;
            }
        }

        private void label22_Click_1(object sender, EventArgs e)
        {
            MEMUC.Text = FilePicker("Executable | *.exe");
            MEMUPath.Text = getDirectory(MEMUC.Text) + Path.AltDirectorySeparatorChar + "MemuHyperv VMs" + Path.AltDirectorySeparatorChar;
            MEMUInstances.Text = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile).Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + Path.AltDirectorySeparatorChar + ".MemuHyperv/MemuHyperv.xml";
        }

        private void MEMUPath_TextChanged_1(object sender, EventArgs e)
        {
            config.MemuPath = MEMUPath.Text;
            if (Directory.Exists(MEMUPath.Text))
            {
                MEMUPath.BackColor = System.Drawing.Color.LightGreen;
            }
            else
            {
                MEMUPath.BackColor = System.Drawing.Color.MistyRose;
            }
        }

        private void gatherCSV_TextChanged_1(object sender, EventArgs e)
        {
            config.GatherCsv = gatherCSV.Text;
            gatherCSVLabel.Text = gatherCSV.Text;
            if (File.Exists(gatherCSV.Text))
            {
                gatherCSV.BackColor = System.Drawing.Color.LightGreen;
            }
            else
            {
                gatherCSV.BackColor = System.Drawing.Color.MistyRose;
            }
        }

        private void label24_Click_1(object sender, EventArgs e)
        {
            gatherCSV.Text = FilePicker("CSV | *.csv");
        }

        private void BackupDir_TextChanged_1(object sender, EventArgs e)
        {
            config.BackupDir = BackupDir.Text;
            if (Directory.Exists(BackupDir.Text))
            {
                BackupDir.BackColor = System.Drawing.Color.LightGreen;
            }
            else
            {
                BackupDir.BackColor = System.Drawing.Color.MistyRose;
            }
        }

        private void label26_Click_1(object sender, EventArgs e)
        {
            BackupDir.Text = DirectoryPicker();
        }

        private void ScreenshotDirSelector_Click(object sender, EventArgs e)
        {
            screenshotDir.Text = DirectoryPicker();
        }

        private void screenshotDir_TextChanged_1(object sender, EventArgs e)
        {
            config.ScreenshotDir = screenshotDir.Text;
            if (Directory.Exists(screenshotDir.Text))
            {
                screenshotDir.BackColor = System.Drawing.Color.LightGreen;
            }
            else
            {
                screenshotDir.BackColor = System.Drawing.Color.MistyRose;
            }
        }

        private void label32_Click_1(object sender, EventArgs e)
        {
            nircmd.Text = FilePicker("Executable | *.exe");
        }

        private void label34_Click_1(object sender, EventArgs e)
        {
            ffmpeg.Text = FilePicker("Executable | *.exe");
        }

        private void nircmd_TextChanged_1(object sender, EventArgs e)
        {
            config.Nircmd = nircmd.Text;
            if (File.Exists(nircmd.Text))
            {
                nircmd.BackColor = System.Drawing.Color.LightGreen;
            }
            else
            {
                nircmd.BackColor = System.Drawing.Color.MistyRose;
            }
        }

        private void ffmpeg_TextChanged_1(object sender, EventArgs e)
        {
            config.Ffmpeg = ffmpeg.Text;
            if (File.Exists(ffmpeg.Text))
            {
                ffmpeg.BackColor = System.Drawing.Color.LightGreen;
            }
            else
            {
                ffmpeg.BackColor = System.Drawing.Color.MistyRose;
            }

        }

        private void ConfigsDir_TextChanged_1(object sender, EventArgs e)
        {
            config.ConfigsDir = ConfigsDir.Text;
            if (Directory.Exists(ConfigsDir.Text))
            {
                ConfigsDir.BackColor = System.Drawing.Color.LightGreen;
            }
            else
            {
                ConfigsDir.BackColor = System.Drawing.Color.MistyRose;
            }
        }

        private void label28_Click_1(object sender, EventArgs e)
        {
            ConfigsDir.Text = DirectoryPicker();
        }

        private void announceStatus_ValueChanged_1(object sender, EventArgs e)
        {
            config.AnnounceStatus = (long)announceStatus.Value;
        }

        private void postStatusScreenshots_CheckedChanged_1(object sender, EventArgs e)
        {
            if (postStatusScreenshots.Checked == true )
            {
                config.PostStatusScreenshots = 1;
            } else
            {
                config.PostStatusScreenshots = 0;
            }
        }

        private void Announce_CheckedChanged_1(object sender, EventArgs e)
        {
            if ( Announce.Checked == true )
            {
                config.Announce = 1;
                Announcement.Enabled = true;
            } else
            {
                config.Announce = 0;
                Announcement.Enabled = false;
            }
        }

        private void DuplicateLog_TextChanged_1(object sender, EventArgs e)
        {
            config.DuplicateLog = DuplicateLog.Text;
            if (File.Exists(DuplicateLog.Text))
            {
                DuplicateLog.BackColor = System.Drawing.Color.LightGreen;
            }
            else
            {
                DuplicateLog.BackColor = System.Drawing.Color.MistyRose;
            }
        }

        private void label30_Click_2(object sender, EventArgs e)
        {
            Form TokenInstructionsForm = new TokenInstructions();
            this.AddOwnedForm(TokenInstructionsForm);
            TokenInstructionsForm.Show(this);
            TokenInstructionsForm.SetDesktopLocation(this.Location.X + 10, this.Location.Y + 10);
        }

        private void useGoogle_CheckedChanged_1(object sender, EventArgs e)
        {
            if ( useGoogle.Checked == true )
            {
                config.UseGoogle = 1;
                GoogleCredentialsFilePath.Enabled = true;
                GoogleTokenFilePath.Enabled = true;
                GoogleSpreadsheetID.Enabled = true;
                GoogleAppName.Enabled = true;
                GoogleSpreadsheetWorksheetName.Enabled = true;
                GoogleButton.Enabled = true;
            } else
            {
                config.UseGoogle = 0;
                GoogleCredentialsFilePath.Enabled = false;
                GoogleTokenFilePath.Enabled = false;
                GoogleSpreadsheetID.Enabled = false;
                GoogleAppName.Enabled = false;
                GoogleSpreadsheetWorksheetName.Enabled = false;
                GoogleButton.Enabled = false;
            }
        }

        private void label49_Click_1(object sender, EventArgs e)
        {
            GoogleCredentialsFilePath.Text = FilePicker("Credentials file | *.json");
        }

        private void GoogleCredentialsFilePath_TextChanged_1(object sender, EventArgs e)
        {
            config.GoogleSecretsFile = GoogleCredentialsFilePath.Text;
            if (File.Exists(GoogleCredentialsFilePath.Text))
            {
                GoogleCredentialsFilePath.BackColor = System.Drawing.Color.LightGreen;
            }
            else
            {
                GoogleCredentialsFilePath.BackColor = System.Drawing.Color.MistyRose;
            }
        }

        private void label50_Click_1(object sender, EventArgs e)
        {
            GoogleTokenFilePath.Text = FilePicker("Token File | *.json");
        }

        private void label44_Click_1(object sender, EventArgs e)
        {
            GNBotLogDir.Text = DirectoryPicker();
            GNBotLogMask.Text = GNBotLogDir.Text + LogFileNameMask.Text;
            GNBotLogMain.Text = GNBotLogDir.Text + MainLogName.Text;
        }

        private void LogFileNameMask_TextChanged(object sender, EventArgs e)
        {
            GNBotLogMask.Text = GNBotLogDir.Text + LogFileNameMask.Text;
        }

        private void MainLogName_TextChanged(object sender, EventArgs e)
        {
            GNBotLogMain.Text = GNBotLogDir.Text + MainLogName.Text;
        }

        private void GNBotLogMain_TextChanged_1(object sender, EventArgs e)
        {
            config.GnBotLogMain = GNBotLogMain.Text;
        }

        private void label19_Click_1(object sender, EventArgs e)
        {
            MEMUPath.Text = DirectoryPicker();
            MEMUInstances.Text = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile).Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + Path.AltDirectorySeparatorChar + ".MemuHyperv/MemuHyperv.xml";
        }

        private void GNBotSettings_TextChanged_1(object sender, EventArgs e)
        {
            config.GnBotSettings = GNBotSettings.Text;
        }

        private void ownerID_TextChanged_1(object sender, EventArgs e)
        {
            config.OwnerId = ownerID.Text;
        }

        private void ownerHandle_TextChanged_1(object sender, EventArgs e)
        {
            config.OwnerHandle = ownerHandle.Text;
        }

        private void Quip_TextChanged_1(object sender, EventArgs e)
        {
            config.Quip = Quip.Text;
        }

        private void Status_TextChanged_1(object sender, EventArgs e)
        {
            config.Status = Status.Text;
        }

        private void Announcement_TextChanged_1(object sender, EventArgs e)
        {
            config.Announcement = Announcement.Text;
        }

        private void Channel_TextChanged_1(object sender, EventArgs e)
        {
            config.Channel = Channel.Text;
        }

        private void GlobalChannel_TextChanged_1(object sender, EventArgs e)
        {
            config.GlobalChannel = GlobalChannel.Text;
        }

        private void GoogleSpreadsheetID_TextChanged_1(object sender, EventArgs e)
        {
            config.GoogleSheetID = GoogleSpreadsheetID.Text;
        }

        private void GNBotLogMask_TextChanged_1(object sender, EventArgs e)
        {
            config.GnBotLogMask = GNBotLogMask.Text;
        }

        private void MEMUInstances_TextChanged_1(object sender, EventArgs e)
        {
            config.MemuInstances = MEMUInstances.Text;
        }

        private void MaximumFailures_ValueChanged(object sender, EventArgs e)
        {
            config.MaxFailures = (long)MaximumFailures.Value;
        }

        private void EnableReboot_CheckedChanged_1(object sender, EventArgs e)
        {
            if ( EnableReboot.Checked == true )
            {
                config.enableReboot = 1;
            } else
            {
                config.enableReboot = 0;
            }
        }

        private void minimumCycleTime_ValueChanged_1(object sender, EventArgs e)
        {
            config.MinimumCycleTime = (long)minimumCycleTime.Value;
            if ( GNBotRestartInterval.Value <= minimumCycleTime.Value )
            {
                GNBotRestartInterval.Value = minimumCycleTime.Value + 1;
            }
        }

        private void GNBotRestartInterval_ValueChanged_1(object sender, EventArgs e)
        {
            config.GNBotRestartInterval = (long)GNBotRestartInterval.Value;
            if ( GNBotRestartInterval.Value <= minimumCycleTime.Value )
            {
                minimumCycleTime.Value = GNBotRestartInterval.Value;
            }
            if ( GNBotRestartInterval.Value > 0 )
            {
                RestartBaseCount.Value = 0;
            }
        }

        private void GNBotRestartFullCycle_CheckedChanged_1(object sender, EventArgs e)
        {
            if ( GNBotRestartFullCycle.Checked == true )
            {
                GNBotRestartInterval.Enabled = false;
                config.GNBotRestartFullCycle = 1;
                config.GNBotRestartInterval = 0;
                RestartBaseCount.Enabled = false;
            } else
            {
                GNBotRestartInterval.Enabled = true;
                RestartBaseCount.Enabled = true;
                config.GNBotRestartFullCycle = 0;
            }
        }

        private void GNBotWindowX_ValueChanged(object sender, EventArgs e)
        {
            config.MoveGNBotWindow[0] = (long)GNBotWindowX.Value;
        }

        private void GNBotWindowY_ValueChanged(object sender, EventArgs e)
        {
            config.MoveGNBotWindow[1] = (long)GNBotWindowY.Value;
        }

        private void GNBotWindowWidth_ValueChanged(object sender, EventArgs e)
        {
            config.MoveGNBotWindow[2] = (long)GNBotWindowWidth.Value;
        }

        private void GNBotWindowHeight_ValueChanged(object sender, EventArgs e)
        {
            config.MoveGNBotWindow[3] = (long)GNBotWindowHeight.Value;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label59_Click(object sender, EventArgs e)
        {
            Form GoogleInstructions = new GoogleHelpForm();
            this.AddOwnedForm(GoogleInstructions);
            GoogleInstructions.Show(this);
            GoogleInstructions.SetDesktopLocation(this.Location.X + 10, this.Location.Y + 10);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void DiscBotOutput_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if ( !File.Exists(DiscBotExePath.Text) )
            {
                DiscBotExePath.Text = FilePicker("Executable | *.exe");
            }
            if (File.Exists(DiscBotExePath.Text))
            {
                
                LaunchDiscBotProcess(DiscBotExePath.Text);
//                discBotRunning = true;
                StopDiscBot.Enabled = true;
                StartDiscBot.Enabled = false;
                LoadButton.Enabled = false;
                SaveButton.Enabled = false;
                BotExeFileSelectButton.Enabled = false;
            }
        }

        private void label64_Click(object sender, EventArgs e)
        {
            DiscBotExePath.Text = FilePicker("Executable | *.exe");
            if (File.Exists(DiscBotExePath.Text) && IsAdmin())
            {
                StartDiscBot.Enabled = true;
                StartDiscBot.Text = "Start";
            } else
            {
                StartDiscBot.Text = "Admin?";
            }
        }
        private void stopDiscBot()
        { // just kill it, there is nothing meaningful to wait for
            try
            {
//                if (discBotRunning) // Don't need this in a try block
//                {
                    DiscBotProcess.Kill();
//                }
                DiscBotProcess.Close();
            }
            catch { }
            discBotRunning = false;
        }
        private void StopDiscBot_Click(object sender, EventArgs e)
        {
            DiscBotOutput.Items.Add("Shutting things down. Please be patient. ");
            StopDiscBot.Enabled = false;
            StopDiscBot.Text = "Stopping";
            StopDiscBot.UseVisualStyleBackColor = true;
            stopDiscBot();
            StartDiscBot.Enabled = true;
            LoadButton.Enabled = true;
            SaveButton.Enabled = true;
            BotExeFileSelectButton.Enabled = true;
            StopDiscBot.Text = "Stop";
            DiscBotOutput.Items.Add("Ready");
        }

        private void DiscBotExePath_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Uri uri = new Uri(ManifestLocation.Text);
            string filename = getFileName(uri.LocalPath);
            using (WebClient wc = new WebClient())
            {
                wc.DownloadProgressChanged += wc_DownloadProgressChanged;
                wc.DownloadFileCompleted += Wc_DownloadFileCompleted;
                wc.DownloadFileAsync(uri, filename);
            }
        }

        private void Wc_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            loadFileManifest();
            DownloadAllButton.Enabled = true;
        }

        // Event to track the progress
        void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }

        string getFileSha1(string file)
        {
            using (FileStream fs = new FileStream(file, FileMode.Open))
            using (BufferedStream bs = new BufferedStream(fs))
            {
                using (SHA1Managed sha1 = new SHA1Managed())
                {
                    byte[] hash = sha1.ComputeHash(bs);
                    StringBuilder formatted = new StringBuilder(2 * hash.Length);
                    foreach (byte b in hash)
                    {
                        formatted.AppendFormat("{0:X2}", b);
                    }
                    return formatted.ToString();
                }
            }
        }
        private void loadFileManifest()
        {
            FileGridView.Rows.Clear();
            Uri uri = new Uri(ManifestLocation.Text);
            string filename = getFileName(uri.LocalPath);
            dynamic filelist = JsonConvert.DeserializeObject(File.ReadAllText(filename));
            // int count = filelist.Count;
            foreach( var item in filelist ) 
            {
                string filestatus = "";
                string fileSHA1 = "";
                bool selected = false;
                Uri fileURI = new Uri((string)item.Value.uri);
                string file = getFileName(fileURI.LocalPath);
                if (File.Exists(file))
                {
                    filestatus = "Current Local File";
                    fileSHA1 = getFileSha1(file);
                    if (fileSHA1 != (string)item.Value.SHA1)
                    {
                        filestatus = "Out of date";
                        selected = true;
                    }
                } else
                {
                    fileSHA1 = (string)item.Value.SHA1;
                    filestatus = "Not Downloaded";
                    selected = true;
                }
                int rowNum = FileGridView.Rows.Add(selected, filestatus, item.Name, fileURI, file, fileSHA1);
            }
        }

        private void DownloadFromQueue()
        {
            if ( _files.Any() )
            {
                string targetFile = _files.Dequeue();
                int rowIndex = 0;
                foreach (DataGridViewRow row in FileGridView.Rows)
                {
                    if (row.Cells["FileUrl"].Value.ToString().Equals(targetFile)) {
                        rowIndex = row.Index;
                    }
                }
                FileGridView.Rows[rowIndex].Cells["DownloadStatus"].Value = "Downloading";
                Uri uri = new Uri(FileGridView.Rows[rowIndex].Cells["FileUrl"].Value.ToString());
                string filename = getFileName(uri.LocalPath);
                DownloadStatusLabel.Text = "Downloading " + uri;
                using (WebClient wc2 = new WebClient())
                {
                    wc2.DownloadProgressChanged += Wc2_DownloadProgressChanged;
                    wc2.DownloadFileCompleted += Wc2_DownloadFileCompleted;
                    wc2.QueryString.Add("fileTracker", targetFile);
                    wc2.DownloadFileAsync(uri, filename);
                }
            } else
            {
                DownloadStatusLabel.Text = "Done!";
            }
        }

        private void DownloadAllButton_Click(object sender, EventArgs e)
        {
            DownloadAllButton.Enabled = false;
            foreach (DataGridViewRow row in FileGridView.Rows)
            {
                if ((bool)row.Cells["DownloadFile"].Value == true)
                {
                    _files.Enqueue(row.Cells["FileUrl"].Value.ToString());
                }
            }
            DownloadFromQueue();
        }

        private void Wc2_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }

        private void Wc2_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            string targetFile = ((System.Net.WebClient)(sender)).QueryString["fileTracker"];
            int rowIndex = 0;
            foreach (DataGridViewRow row in FileGridView.Rows)
            {
                if (row.Cells["FileUrl"].Value.ToString().Equals(targetFile))
                {
                    rowIndex = row.Index;
                }
            }
            FileGridView.Rows[rowIndex].Cells["DownloadStatus"].Value = "Downloaded";
            DownloadFromQueue();
        }

        private void FileGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void DebugLevel_ValueChanged(object sender, EventArgs e)
        {
            config.Debug = (int)DebugLevel.Value;
        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            if ( debugEnableCheckbox.Checked == true) {
                DebugLevel.Enabled = true;
            } else
            {
                DebugLevel.Enabled = false;
                DebugLevel.Value = 0;
            }
            config.Debug = (int)DebugLevel.Value;
        }

        private void BasicConfig_Click(object sender, EventArgs e)
        {

        }
        private void BasicConfig_SelectedIndexChanged(object sender, System.EventArgs e)
        {

        }

        private void GoogleSheet_Click(object sender, EventArgs e)
        {

        }
        private void GoogleSheet_SelectedIndexChanged(object sender, System.EventArgs e)
        {

        }

        private void AdvancedConfig_Click(object sender, EventArgs e)
        {

        }
        private void AdvancedConfig_SelectedIndexChanged(object sender, System.EventArgs e)
        {

        }

        private void DownloadTab_Click(object sender, EventArgs e)
        {

        }
        private void DownloadTab_SelectedIndexChanged(object sender, System.EventArgs e)
        {

        }

        private void RunDiscBotTab_Click(object sender, EventArgs e)
        {

        }
        private void RunDiscBotTab_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            // If we are running capture the tab

            if (discBotRunning)
            {
                ConfigurationTabs.SelectedTab = RunDiscBotTab;
                StopDiscBot.BackColor = System.Drawing.Color.LightGreen;
            }
        }

        private void label39_Click(object sender, EventArgs e)
        {

        }

        private void EnableGatherCSV_CheckedChanged(object sender, EventArgs e)
        {
            if ( EnableGatherCSV.Checked == true )
            {
                gatherCSV.Enabled = true;
                GatherCSVFilePicker.Enabled = true;
            } else
            {
                gatherCSV.Enabled = false;
                gatherCSV.Text = "";
                GatherCSVFilePicker.Enabled = false;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            config.GameDayMap["1"].Label = Day1Label.Text;
        }

        private void label29_Click(object sender, EventArgs e)
        {
            Day1Profile.Text = getFileName(FilePicker("GNBot Profile | *.json")).Replace(".json", "");
        }

        private void label57_Click(object sender, EventArgs e)
        {
            Day2Profile.Text = getFileName(FilePicker("GNBot Profile | *.json")).Replace(".json", "");
        }

        private void label65_Click(object sender, EventArgs e)
        {
            Day3Profile.Text = getFileName(FilePicker("GNBot Profile | *.json")).Replace(".json", "");
        }

        private void label67_Click(object sender, EventArgs e)
        {
            Day4Profile.Text = getFileName(FilePicker("GNBot Profile | *.json")).Replace(".json", "");
        }

        private void label69_Click(object sender, EventArgs e)
        {
            Day5Profile.Text = getFileName(FilePicker("GNBot Profile | *.json")).Replace(".json", "");
        }

        private void label71_Click(object sender, EventArgs e)
        {
            Day6Profile.Text = getFileName(FilePicker("GNBot Profile | *.json")).Replace(".json", "");
        }

        private void label73_Click(object sender, EventArgs e)
        {
            Day0Profile.Text = getFileName(FilePicker("GNBot Profile | *.json")).Replace(".json", "");
        }

        private void Day1Profile_TextChanged(object sender, EventArgs e)
        {
            config.GameDayMap["1"].Profile = Day1Profile.Text;
        }

        private void Day2Profile_TextChanged(object sender, EventArgs e)
        {
            config.GameDayMap["2"].Profile = Day2Profile.Text;
        }

        private void Day3Profile_TextChanged(object sender, EventArgs e)
        {
            config.GameDayMap["3"].Profile = Day3Profile.Text;
        }

        private void Day4Profile_TextChanged(object sender, EventArgs e)
        {
            config.GameDayMap["4"].Profile = Day4Profile.Text;
        }

        private void Day5Profile_TextChanged(object sender, EventArgs e)
        {
            config.GameDayMap["5"].Profile = Day5Profile.Text;
        }

        private void Day6Profile_TextChanged(object sender, EventArgs e)
        {
            config.GameDayMap["6"].Profile = Day6Profile.Text;
        }

        private void Day0Profile_TextChanged(object sender, EventArgs e)
        {
            config.GameDayMap["0"].Profile = Day0Profile.Text;
        }

        private void Day2Label_TextChanged(object sender, EventArgs e)
        {
            config.GameDayMap["2"].Label = Day2Label.Text;
        }

        private void Day3Label_TextChanged(object sender, EventArgs e)
        {
            config.GameDayMap["3"].Label = Day3Label.Text;
        }

        private void Day4Label_TextChanged(object sender, EventArgs e)
        {
            config.GameDayMap["4"].Label = Day4Label.Text;
        }

        private void Day5Label_TextChanged(object sender, EventArgs e)
        {
            config.GameDayMap["5"].Label = Day5Label.Text;
        }

        private void Day6Label_TextChanged(object sender, EventArgs e)
        {
            config.GameDayMap["6"].Label = Day6Label.Text;
        }

        private void Day0Label_TextChanged(object sender, EventArgs e)
        {
            config.GameDayMap["0"].Label = Day0Label.Text;
        }

        private void ActivateBaseTime_ValueChanged(object sender, EventArgs e)
        {
            config.ActiveBaseTimer = (long)ActivateBaseTime.Value;
        }

        private void label80_Click(object sender, EventArgs e)
        {
            PausedMasterFile.Text = FilePicker("GNBot Profile | *.json");
            config.PausedMaster = PausedMasterFile.Text;
        }

        private void checkBox1_CheckedChanged_2(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                PausedMasterFile.Enabled = true;
                ActivateBaseTime.Enabled = true;
                ActivateBaseTime.Value = 60;
                PausedMasterFilePicker.Enabled = true;
            } else
            {
                PausedMasterFile.Enabled = false;
                PausedMasterFile.Text = "";
                ActivateBaseTime.Enabled = false;
                ActivateBaseTime.Value = 0;
                PausedMasterFilePicker.Enabled = false;
            }
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            Form CSVEditorForm = new CSVEditor(this);
            this.AddOwnedForm(CSVEditorForm);
            CSVEditorForm.Show(this);
            CSVEditorForm.SetDesktopLocation(this.Location.X + 10, this.Location.Y + 10);
        }
        private void PausedMasterFile_TextChanged(object sender, EventArgs e)
        {
            config.PausedMaster = PausedMasterFile.Text;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                CSVEditButton.Enabled = true;
            }
            else
            {
                CSVEditButton.Enabled = false;
            }
        }

        private void RestartBaseCount_ValueChanged(object sender, EventArgs e)
        {
            if ( RestartBaseCount.Value > 0 )
            {
                GNBotRestartInterval.Value = 0;
            }
        }

        private void MyDiscBotConfigForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (discBotRunning)
            {
                DiscBotProcess.Kill();
            }
        }

        private void label45_Click_1(object sender, EventArgs e)
        {
            if (label45.Text == "***")
            {
                GoogleSpreadsheetID.UseSystemPasswordChar = true;
                label45.Text = "ABC";
            }
            else
            {
                GoogleSpreadsheetID.UseSystemPasswordChar = false;
                label45.Text = "***";
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (CloudLogEnable.Checked == true )
            {
                config.cloudLogs.Enabled = 1;
                CloudLogSource.Enabled = true;
                CloudLogToken.Enabled = true;
                CloudLogHost.Enabled = true;
                CloudLogEndpoint.Enabled = true;
                CloudLogPort.Enabled = true;
                CloudLogInitFunction.Enabled = true;
                CloudLogSubmitFunction.Enabled = true;
                CloudLogModuleSelector.Enabled = true;
            } else
            {
                config.cloudLogs.Enabled = 0;
                CloudLogSource.Enabled = false;
                CloudLogToken.Enabled = false;
                CloudLogHost.Enabled = false;
                CloudLogEndpoint.Enabled = false;
                CloudLogPort.Enabled = false;
                CloudLogInitFunction.Enabled = false;
                CloudLogSubmitFunction.Enabled = false;
                CloudLogModuleSelector.Enabled = true;
            }
        }

        private void label81_Click(object sender, EventArgs e)
        {
            CloudLogSource.Text = FilePicker("Module | *.js");
        }

        private void CloudLogToken_TextChanged(object sender, EventArgs e)
        {
            config.cloudLogs.Token = CloudLogToken.Text;
        }

        private void CloudLogSource_TextChanged(object sender, EventArgs e)
        {
            config.cloudLogs.Source = CloudLogSource.Text;
        }

        private void CloudLogHost_TextChanged(object sender, EventArgs e)
        {
            config.cloudLogs.Host = CloudLogHost.Text;
        }

        private void CloudLogPort_ValueChanged(object sender, EventArgs e)
        {
            config.cloudLogs.Port = (long)CloudLogPort.Value;
        }

        private void CloudLogEndpoint_TextChanged(object sender, EventArgs e)
        {
            config.cloudLogs.Endpoint = CloudLogEndpoint.Text;
        }

        private void CloudLogSubmitFunction_TextChanged(object sender, EventArgs e)
        {
            config.cloudLogs.Submit = CloudLogSubmitFunction.Text;
        }

        private void CloudLogInitFunction_TextChanged(object sender, EventArgs e)
        {
            config.cloudLogs.Init = CloudLogInitFunction.Text;
        }
    }
}
