using BackgroundApplication2.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using System.IO;
using System.Threading;

namespace NamenschildDesktop
{
    /// <summary>
    /// Interaktionslogik für Progressbar_1.xaml
    /// </summary>
    public partial class Progressbar_1 : Window
    {
        public Progressbar_1()
        {
            InitializeComponent();
        }


        public void SetLabel(string text)
        {
            ProgressLabel.Text = text;
        }

        public void AddProgress(int progress)
        {
            Progress.Value += progress;
        }

        public async static Task InstallApp(string ip)
        {

            try
            {
                Progressbar_1 pg = new Progressbar_1();

                const int progress = 100 / 8;

                pg.Show();
                pg.BringIntoView();

                pg.SetLabel("Vorbereitung");
                string root = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName, "IoT-Apps");

                pg.SetLabel("Lade Abhängigkeiten hoch");
                string file5 = root + "/Microsoft.NET.CoreRuntime.1.1.appx";
                await HardwareController.UploadApp(ip, file5, Path.GetFileName(file5));

                pg.SetLabel("Warte auf die Installation der Abhängigkeiten");
                await HardwareController.UploadFinished(ip);

                pg.SetLabel("Lade Abhängigkeiten hoch");
                string file6 = root + "/Microsoft.VCLibs.ARM.Debug.14.00.appx";
                await HardwareController.UploadApp(ip, file6, Path.GetFileName(file6));

                pg.SetLabel("Warte auf die Installation der Abhängigkeiten");
                await HardwareController.UploadFinished(ip);

                pg.SetLabel("Lade Vordergrund Applikation hoch");
                string file2 = root + "/ForeGround_App_1.0.0.0_x86_x64_arm_Debug.appxbundle";
                await HardwareController.UploadApp(ip, file2, Path.GetFileName(file2));

                pg.AddProgress(progress);
                pg.SetLabel("Warte auf die Installation der Vordergrund Applikation");
                await HardwareController.UploadFinished(ip);

                pg.AddProgress(progress);
                pg.SetLabel("Lade Vordergrund Prüfungsapplikation hoch");
                string file3 = root + "/ForeGround_Prufung_1.0.0.0_x86_x64_arm_Debug.appxbundle";
                await HardwareController.UploadApp(ip, file3, Path.GetFileName(file3));

                pg.AddProgress(progress);
                pg.SetLabel("Warte auf die Installation der Prüfungsapplikation");
                await HardwareController.UploadFinished(ip);

                pg.AddProgress(progress);
                pg.SetLabel("Lade Kommunikations-APP hoch");
                string file4 = root + "/UwpMessageRelay_1.0.0.0_x86_x64_arm_Debug.appxbundle";
                await HardwareController.UploadApp(ip, file4, Path.GetFileName(file4));

                pg.AddProgress(progress);
                pg.SetLabel("Warte auf Fertigstellung");
                await HardwareController.UploadFinished(ip);

                pg.AddProgress(progress);
                pg.SetLabel("Lade Hintergrund API hoch");
                string file1 = root + "/BackgroundApplication2_1.0.0.0_x86_x64_arm_Debug.appxbundle";
                await HardwareController.UploadApp(ip, file1, Path.GetFileName(file1));

                pg.AddProgress(progress);
                pg.SetLabel("Warte auf die Installation der Hintergrund API");
                await HardwareController.UploadFinished(ip);

                pg.AddProgress(progress);
                pg.SetLabel("Setze APPs als Standard APPs");
                await HardwareController.StartupHeadlessApp(ip, "BackgroundApplication2-uwp_dfdr0qb8gsad2!App");
                await HardwareController.SetDefault(ip, "Foreground_dfdr0qb8gsad2!App");

                Thread.Sleep(1000);
                pg.AddProgress(progress);

                pg.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler: Die Installation konnte nicht vollständig ausgeführt werden. Originaler Fehler: " + ex.Message);
            }
        }

        public async static Task UninstallApp(string ip)
        {
            Progressbar_1 pg = new Progressbar_1();
            const int progress = 100 / 4;

            pg.Show();
            pg.InitializeComponent();

            pg.SetLabel("Vorbereitung");
            string name = "Foreground_1.0.0.0_arm__dfdr0qb8gsad2";
            pg.SetLabel("Deinstalliere Vordergrund Applikation");
            await HardwareController.DeleteAPP(ip, name);

            pg.AddProgress(progress);
            pg.SetLabel("Deinstalliere Hintergrund Applikation");
            name = "BackgroundApplication2-uwp_1.0.0.0_arm__dfdr0qb8gsad2";
            await HardwareController.StopHeadlessApp(ip,name);
            await HardwareController.DeleteAPP(ip, name);

            pg.AddProgress(progress);
            pg.SetLabel("Deinstalliere UWP Relay");
            name = "UwpMessageRelay-uwp_1.0.0.0_arm__dfdr0qb8gsad2";
            await HardwareController.DeleteAPP(ip, name);

            pg.AddProgress(progress);
            pg.SetLabel("Deinstalliere Prüfungsapplikation");
            name = "ForeGroundPrufung_1.0.0.0_arm__dfdr0qb8gsad2";
            await HardwareController.DeleteAPP(ip, name);

            pg.SetLabel("Fertig");
            pg.AddProgress(progress);

            pg.Close();

        }


    }
}
