using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.IO;




using Common;
using Common.Utils;

namespace InstallTerminal
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IClientInstaller
    {

        private  CInstaller _installer;

        private EnmStates _currState;



        private System.Windows.Threading.Dispatcher _disp;




        public MainWindow()
        {
            InitializeComponent();

            _disp = System.Windows.Threading.Dispatcher.CurrentDispatcher;

             
            string[] args = Environment.GetCommandLineArgs();


            bool bUnInstall = args.Length >= 2 && args[1] ==  "/uninstall";

            if (bUnInstall)
                DoUnInstall();
            else
                DoInstall();



        }
        private bool _showGridFlags = true;


        private void DoInstall()
        {


            Title += " Версия: " + CUtil.GetVersion();



            _currState = EnmStates._01_Initial;


            _installer = new CInstaller(this);


            if ( _installer.PathConfigDir == null) //env not set
            {
                checkBoxServersConf.IsChecked = true;
                checkBoxInstrumentsConf.IsChecked = true;
                checkBoxStockVisual.IsChecked = true;
                checkBoxTerminalConf.IsChecked = true;
                _showGridFlags = false;

            }
            else //env is set
            {
                if (!Directory.Exists(_installer.PathTerminalConfigDir))//conf dir is not exists
                {

                    checkBoxServersConf.IsChecked = true;                    
                    checkBoxInstrumentsConf.IsChecked = true;
                    checkBoxStockVisual.IsChecked = true;
                    checkBoxTerminalConf.IsChecked = true;
                    _showGridFlags = false;
                }
                else
                {
                    string pathServConf = String.Format(@"{0}\\servers.xml", _installer.PathTerminalConfigDir);
                    if (!File.Exists(pathServConf))
                        checkBoxServersConf.IsChecked = true;

                    if (!Directory.Exists(_installer.PathInstrumentsConfDir))
                        checkBoxInstrumentsConf.IsChecked = true;

                    if (!File.Exists(_installer.PathVisualStockFile))
                        checkBoxStockVisual.IsChecked = true;

                    if (!File.Exists(_installer.PathTerminalConfFile))
                       checkBoxTerminalConf.IsChecked = true;


                }


              


            }

            
            //_installer = 


            /*
            if (NeedSetCheckBoxes())
            {
                checkBoxServersConf.IsChecked = true;
                checkBoxTerminalConf.IsChecked = true;
            }
            */
          

            //2018-07-28
            if (_installer.PathTerminalExecDir == null)
                SetTextBoxValue(TextBoxPathTerminalExec, _installer.GetDefaultExecPath());
            else
                SetTextBoxValue(TextBoxPathTerminalExec, _installer.PathTerminalExecDir);

            if (_installer.PathConfigDir == null)
                SetTextBoxValue(TextBoxPathTerminalConfig, _installer.GetDefaultRootDataPath() + @"\Config\Terminal");           
            else
                SetTextBoxValue(TextBoxPathTerminalConfig, _installer.PathConfigDir);

            if (_installer.PathLog == null)
                SetTextBoxValue(TextBoxPathLogs, _installer.GetDefaultRootDataPath() + @"\Logs\Terminal");
            else
                SetTextBoxValue(TextBoxPathLogs, _installer.PathLog);

            if (_installer.PathData == null)
                SetTextBoxValue(TextBoxPathData, _installer.GetDefaultRootDataPath() + @"\Data\Terminal");
            else
                SetTextBoxValue(TextBoxPathData, _installer.PathData);

            if (_installer.PathAlarm == null)
                SetTextBoxValue(TextBoxPathAlarm, _installer.GetDefaultRootDataPath() + @"\Alarms\Terminal");
            else
                SetTextBoxValue(TextBoxPathAlarm, _installer.PathAlarm);

            if (_installer.PathTemp == null)
                SetTextBoxValue(TextBoxPathTemp, _installer.GetDefaultRootDataPath() + @"\Temp\Terminal");
            else
                SetTextBoxValue(TextBoxPathTemp, _installer.PathTemp);


            if (!_installer.IsAnyEnvSet())
            {
                SetVisibleAdvanceData();

            }
        }


     


        




        private void DoUnInstall()
        {

            // GridPathes.Visibility = Visibility.Hidden;
            // ButtonContinue.Visibility = Visibility.Hidden;
            WindowUninstall winUninstall = new WindowUninstall();
            winUninstall.Show();


            this.Close();




        }





        private void UpdateDependendDataTextBoxes()
        {


            SetTextBoxValue(TextBoxPathTerminalConfig, Concat(TextBoxPathTerminalDataRoot.Text,  @"\Config"));
            SetTextBoxValue(TextBoxPathLogs, Concat(TextBoxPathTerminalDataRoot.Text,  @"\Logs"));
            SetTextBoxValue(TextBoxPathData, Concat(TextBoxPathTerminalDataRoot.Text, @"\Data"));
            SetTextBoxValue(TextBoxPathAlarm, Concat(TextBoxPathTerminalDataRoot.Text, @"\Alarms"));
            SetTextBoxValue(TextBoxPathTemp, Concat(TextBoxPathTerminalDataRoot.Text, @"\Temp"));

        }

        private string Concat(string rootPath, string suffix)
        {
            return rootPath.Last() == '\\' ? rootPath.Substring(0, rootPath.Length - 1) + suffix : rootPath + suffix;
        }












        private void ShowWindowKillTerminal()
        {

            WindowKillTerminal win = new WindowKillTerminal(TaskStartInstall);

            CUtilWin.ShowDialogOnCenter(win, this);

        }
        



        private void SetTextBoxValue(TextBox textBox, string path  )
        {
           
            if (path != null)
                textBox.Text =path;

        }



        private bool IsValidPathField(TextBox textBox, string errorMessage )
        {
            string path = textBox.Text;

            if (path == null ||
                path == "")
            {
                ShowErrorWin(errorMessage);
                return false;
            }
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

          


            return true;

        }


        private string GetVar(string var)
        {
            return Environment.GetEnvironmentVariable(var, EnvironmentVariableTarget.Machine);
        }


        private bool NeedSetCheckBoxes()
        {

            if (GetVar("TERMINAL_PATH") == null ||
                GetVar("CONFIG_PATH") == null ||
                GetVar("LOG_PATH") == null ||   
                GetVar("DATA_PATH") == null)
                    return  true;


            return false;
        }




        private void ButtonContinue_Click(object sender, RoutedEventArgs e)
        {
            //AFTER path filled do show chaeckboxes (but check conditions first)
            if (EnmStates._01_Initial == _currState)
            {

                if (!IsValidPathField(TextBoxPathTerminalExec, "Каталог терминала не задан") ||
                    !IsValidPathField(TextBoxPathTerminalConfig, "Каталог конфигурации не задан") ||
                    !IsValidPathField(TextBoxPathLogs, "Каталог лог-файлов не задан") ||
                    !IsValidPathField(TextBoxPathData, "Каталог с данными не задан") ||
                    !IsValidPathField(TextBoxPathAlarm, "Каталог с аварийными сообщениями не задан") ||
                    !IsValidPathField(TextBoxPathTemp, "Каталог с временными файлами не задан"))
                {
                    return;

                }


                if (CUtil.GetProcess("Terminal") != null)
                {
                    ShowWindowKillTerminal();
                    return;

                }

                StackPanelPathes.Visibility = Visibility.Collapsed;
                StackPanelFlags.Visibility = Visibility.Visible;

                if (!_showGridFlags)
                {
                    GroupBoxCheckBoxes.Visibility = Visibility.Collapsed;
                    //CUtil.TaskStart(TaskStartInstall);
                    //return;
                }
             

                SetState(EnmStates._02_Pathes_filled);

            }
            //AFTER checkboxes selected do start installation process
            else if (EnmStates._02_Pathes_filled == _currState)
            {
                StackPanelFlags.Visibility = Visibility.Collapsed;
                StackPanelInstall.Visibility = Visibility.Visible;
               

                CUtil.TaskStart(TaskStartInstall);


            }


         
        
        }


        private void SetState(EnmStates newState)
        {

            _currState = newState;

        }


        private void TaskStartInstall()
        {

            try
            {
                _disp.BeginInvoke(new Action(() => ButtonContinue.IsEnabled = false));



                OutMessage("Начало установки");


                OutMessage("Добавление переменных среды");
                UpdateProgressBar(2);

                ProcessOneEnvVar("TERMINAL_PATH", TextBoxPathTerminalExec);
                // _installer.ProcessOneEnvVar("TERMINAL_PATH", TextBoxPathTerminalExec.Text);
                UpdateProgressBar(3);


                //_installer.ProcessOneEnvVar("CONFIG_PATH", TextBoxPathTerminalConfig.Text);
                ProcessOneEnvVar("CONFIG_PATH", TextBoxPathTerminalConfig);
                UpdateProgressBar(5);

                //_installer.ProcessOneEnvVar("LOG_PATH", TextBoxPathLogs.Text);
                ProcessOneEnvVar("LOG_PATH", TextBoxPathLogs);
                UpdateProgressBar(7);

                //_installer.ProcessOneEnvVar("DATA_PATH", TextBoxPathData.Text);
                ProcessOneEnvVar("DATA_PATH", TextBoxPathData);
                UpdateProgressBar(9);

                //_installer.ProcessOneEnvVar("ALARMS_PATH", TextBoxPathAlarm.Text);
                ProcessOneEnvVar("ALARMS_PATH", TextBoxPathAlarm);
                UpdateProgressBar(11);

                //_installer.ProcessOneEnvVar("TEMP_PATH", TextBoxPathTemp.Text);
                ProcessOneEnvVar("TEMP_PATH", TextBoxPathTemp);
                UpdateProgressBar(13);

                OutMessage("Обновлены переменные среды");


                _disp.Invoke(new Action(() =>
              {
                  _installer.IsOverwriteTerminalConfig = (bool)checkBoxTerminalConf.IsChecked;

                  _installer.IsOverwriteServerConfig = (bool)checkBoxServersConf.IsChecked;

                  _installer.IsOverwriteStockVisualConfig = (bool)checkBoxStockVisual.IsChecked;

                  _installer.IsOverwriteInstrumentsConfig = (bool)checkBoxInstrumentsConf.IsChecked;

                  _installer.IsNeedCreateShortCut = checkBoxCreateShortCut.IsChecked == true ? true : false;
              }
                ));

                _installer.Install();


                _disp.BeginInvoke(new Action(() =>
                {
                    ListBoxMessages.ScrollIntoView(ListBoxMessages.Items.CurrentItem);
                   // ButtonContinue.IsEnabled = true;
                }
               ));




            }
            catch (Exception e)
            {
                CallBackErrorExit("Внутренняя ошибка установки");

            }

        }

       




        private void ProcessOneEnvVar(string env, TextBox textBox)
        {
            string text = "";

            _disp.Invoke(new Action(() => text = textBox.Text));
            _installer.ProcessOneEnvVar(env, text);




        }




       


        public void CallBackErrorExit(string error)
        {
            _disp.Invoke(new Action(() =>
           {
               ShowErrorWin(error);
               Close();
           }
            ));
        }




        private void ShowErrorWin(string error)
        {
            ErrorWin errWin = new ErrorWin();
            errWin.LabelError.Text = error;
            CUtilWin.ShowDialogOnCenter(errWin, this);

            


        }



        public void CallbackSuccess(string msg)
        {
            _disp.BeginInvoke(new Action(() =>
            {
                SuccessWin succWin = new SuccessWin();
                succWin.TextBlockMessage.Text = msg;
                CUtilWin.ShowDialogOnCenter(succWin, this);
                succWin.Activate();
                Close();
            }));

        }

        private void ButtonTerminalExec_Click(object sender, RoutedEventArgs e)
        {
            SelectDirectory(TextBoxPathTerminalExec);               
        }


     


        private void ButtonTerminaConf_Click(object sender, RoutedEventArgs e)
        {
            SelectDirectory(TextBoxPathTerminalConfig);

        }


        private void ButtonLogs_Click(object sender, RoutedEventArgs e)
        {

            SelectDirectory(TextBoxPathLogs);
        }


        private void ButtonData_Click(object sender, RoutedEventArgs e)
        {
            SelectDirectory(TextBoxPathData);

        }

        private void ButtonAlarms_Click(object sender, RoutedEventArgs e)
        {
            SelectDirectory(TextBoxPathAlarm);

        }

        private void ButtonTemp_Click(object sender, RoutedEventArgs e)
        {
            SelectDirectory(TextBoxPathTemp);

        }




        private void SelectDirectory(TextBox textBlock)
        {

            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                dialog.SelectedPath = textBlock.Text;
               // dialog.RootFolder = Environment.SpecialFolder.MyComputer;
              
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                    textBlock.Text = dialog.SelectedPath;


            }
        }

        public void UpdateProgressBar(int value)
        {
            _disp.BeginInvoke(new Action( ()=>
           {
               ProgressBarTotal.Visibility = Visibility.Visible;
               ProgressBarTotal.Value = value;

               TextBlockProgress.Visibility = Visibility.Visible;
               TextBlockProgress.Text = value.ToString() + " %";
           }
            ));
        }

        public void OutMessage(string msg)
        {
            _disp.BeginInvoke(new Action (() =>
                                ListBoxMessages.Items.Add(msg)
                             ));
        }

        private void ButtonAdvanced_Click(object sender, RoutedEventArgs e)
        {
            if (GridTerminalData.Visibility == Visibility.Collapsed)            
				SetVisibleAdvanceData();            
            else if (GridTerminalData.Visibility == Visibility.Visible)
				SetInvisilbeAdvanceData();
              

        }


		private void SetVisibleAdvanceData()
		{

			GridTerminalData.Visibility = Visibility.Visible;
			ButtonAdvanced.Content = "Свернуть";
		}




		private void SetInvisilbeAdvanceData()
		{

			  GridTerminalData.Visibility = Visibility.Collapsed;
                ButtonAdvanced.Content = "Дополнительно";
           

		}

        private void TextBoxPathTerminalDataRoot_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateDependendDataTextBoxes();
        }

        private void ButtonTerminalDataRoot_Click(object sender, RoutedEventArgs e)
        {
            SelectDirectory(TextBoxPathTerminalDataRoot);
        }

     






     



    }
}
