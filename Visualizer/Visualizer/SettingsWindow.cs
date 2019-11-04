namespace Visualizer
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Markup;
    using System.Windows.Media;
    using Xceed.Wpf.Toolkit;
    //TODO move to terminal or revmoce
    public class SettingsWindow : Window, IComponentConnector
    {
        private bool _contentLoaded;
        internal Button Buttn_AddLeader;
        internal Button Buttn_AutoScroll;
        internal ComboBox Buttn_AveragingMethod;
        internal ComboBox Buttn_ClustersTF;
        internal ComboBox Buttn_ClusterStyleColor;
        internal ComboBox Buttn_ClusterStyleText;
        internal Button Buttn_RemoveLeader;
        internal Button Buttn_SaveClusters;
        internal Button Buttn_ShowClusterPanel;
        internal Button Buttn_ShowLeadersNames;
        internal ComboBox Buttn_TicksStyle;
        internal ComboBox Buttn_VertVolumeStyle;
        internal Grid Grid_Clusters;
        internal Grid Grid_DOM;
        internal Grid Grid_Ticks;
        internal Grid Grid_Trading;
        private DateTime m_dEnterTime = DateTime.Now;
        private DateTime m_dExitTime = DateTime.Now;
        internal TabControl SettingsTabs;

        public SettingsWindow()
        {
            this.InitializeComponent();
            this.CreateOtherButtons();
        }

        private void CreateOtherButtons()
        {
            IntegerUpDown down = new IntegerUpDown {
                Name = "Buttn_FirstWorkAmount",
                Tag = "Buttn_FirstWorkAmount",
                Background = Brushes.Transparent,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Center,
                TextAlignment = TextAlignment.Center,
                Minimum = 1,
                Maximum = 0x2710,
                Value = 1,
                Margin = new Thickness(16.0, 0.0, 16.0, 0.0)
            };
            this.Buttn_FirstWorkAmount = down;
            Grid.SetColumn(this.Buttn_FirstWorkAmount, 1);
            Grid.SetRow(this.Buttn_FirstWorkAmount, 0);
            this.Grid_Trading.Children.Add(this.Buttn_FirstWorkAmount);
            IntegerUpDown down2 = new IntegerUpDown {
                Name = "Buttn_SecondWorkAmount",
                Tag = "Buttn_SecondWorkAmount",
                Background = Brushes.Transparent,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Center,
                TextAlignment = TextAlignment.Center,
                Minimum = 1,
                Maximum = 0x2710,
                Value = 1,
                Margin = new Thickness(16.0, 0.0, 16.0, 0.0)
            };
            this.Buttn_SecondWorkAmount = down2;
            Grid.SetColumn(this.Buttn_SecondWorkAmount, 1);
            Grid.SetRow(this.Buttn_SecondWorkAmount, 1);
            this.Grid_Trading.Children.Add(this.Buttn_SecondWorkAmount);
            IntegerUpDown down3 = new IntegerUpDown {
                Name = "Buttn_ThirdWorkAmount",
                Tag = "Buttn_ThirdWorkAmount",
                Background = Brushes.Transparent,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Center,
                TextAlignment = TextAlignment.Center,
                Minimum = 1,
                Maximum = 0x2710,
                Value = 1,
                Margin = new Thickness(16.0, 0.0, 16.0, 0.0)
            };
            this.Buttn_ThirdWorkAmount = down3;
            Grid.SetColumn(this.Buttn_ThirdWorkAmount, 1);
            Grid.SetRow(this.Buttn_ThirdWorkAmount, 2);
            this.Grid_Trading.Children.Add(this.Buttn_ThirdWorkAmount);
            IntegerUpDown down4 = new IntegerUpDown {
                Name = "Buttn_FirstIncreaseAmount",
                Tag = "Buttn_FirstIncreaseAmount",
                Background = Brushes.Transparent,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Center,
                TextAlignment = TextAlignment.Center,
                Minimum = 1,
                Maximum = 0x2710,
                Value = 1,
                Margin = new Thickness(16.0, 0.0, 16.0, 0.0)
            };
            this.Buttn_FirstIncreaseAmount = down4;
            Grid.SetColumn(this.Buttn_FirstIncreaseAmount, 1);
            Grid.SetRow(this.Buttn_FirstIncreaseAmount, 3);
            this.Grid_Trading.Children.Add(this.Buttn_FirstIncreaseAmount);
            IntegerUpDown down5 = new IntegerUpDown {
                Name = "Buttn_SecondIncreaseAmount",
                Tag = "Buttn_SecondIncreaseAmount",
                Background = Brushes.Transparent,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Center,
                TextAlignment = TextAlignment.Center,
                Minimum = 1,
                Maximum = 0x2710,
                Value = 1,
                Margin = new Thickness(16.0, 0.0, 16.0, 0.0)
            };
            this.Buttn_SecondIncreaseAmount = down5;
            Grid.SetColumn(this.Buttn_SecondIncreaseAmount, 1);
            Grid.SetRow(this.Buttn_SecondIncreaseAmount, 4);
            this.Grid_Trading.Children.Add(this.Buttn_SecondIncreaseAmount);
            IntegerUpDown down6 = new IntegerUpDown {
                Name = "Buttn_DecreaseAmount",
                Tag = "Buttn_DecreaseAmount",
                Background = Brushes.Transparent,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Center,
                TextAlignment = TextAlignment.Center,
                Minimum = 1,
                Maximum = 0x2710,
                Value = 1,
                Margin = new Thickness(16.0, 0.0, 16.0, 0.0)
            };
            this.Buttn_DecreaseAmount = down6;
            Grid.SetColumn(this.Buttn_DecreaseAmount, 1);
            Grid.SetRow(this.Buttn_DecreaseAmount, 5);
            this.Grid_Trading.Children.Add(this.Buttn_DecreaseAmount);
            IntegerUpDown down7 = new IntegerUpDown {
                Name = "Buttn_MaxPosition",
                Tag = "Buttn_MaxPosition",
                Background = Brushes.Transparent,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Center,
                TextAlignment = TextAlignment.Center,
                Minimum = 1,
                Maximum = 0x2710,
                Value = 3,
                Margin = new Thickness(16.0, 0.0, 16.0, 0.0)
            };
            this.Buttn_MaxPosition = down7;
            Grid.SetColumn(this.Buttn_MaxPosition, 1);
            Grid.SetRow(this.Buttn_MaxPosition, 6);
            this.Grid_Trading.Children.Add(this.Buttn_MaxPosition);
            IntegerUpDown down8 = new IntegerUpDown {
                Name = "Buttn_BackAmount",
                Tag = "Buttn_BackAmount",
                Background = Brushes.Transparent,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Center,
                TextAlignment = TextAlignment.Center,
                Minimum = 1,
                Maximum = 0x1869f,
                Value = 5,
                Margin = new Thickness(16.0, 0.0, 16.0, 0.0)
            };
            this.Buttn_BackAmount = down8;
            Grid.SetColumn(this.Buttn_BackAmount, 1);
            Grid.SetRow(this.Buttn_BackAmount, 7);
            this.Grid_Trading.Children.Add(this.Buttn_BackAmount);
            IntegerUpDown down9 = new IntegerUpDown {
                Name = "Buttn_ThrowLimitTo",
                Tag = "Buttn_ThrowLimitTo",
                Background = Brushes.Transparent,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Center,
                TextAlignment = TextAlignment.Center,
                Minimum = 1,
                Maximum = 0x3e8,
                Value = 40,
                Margin = new Thickness(16.0, 0.0, 16.0, 0.0)
            };
            this.Buttn_ThrowLimitTo = down9;
            Grid.SetColumn(this.Buttn_ThrowLimitTo, 1);
            Grid.SetRow(this.Buttn_ThrowLimitTo, 8);
            this.Grid_Trading.Children.Add(this.Buttn_ThrowLimitTo);
            IntegerUpDown down10 = new IntegerUpDown {
                Name = "Buttn_StopLossSteps",
                Tag = "Buttn_StopLossSteps",
                Background = Brushes.Transparent,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Center,
                TextAlignment = TextAlignment.Center,
                Minimum = 0,
                Maximum = 0x1869f,
                Value = 0,
                Margin = new Thickness(16.0, 0.0, 16.0, 0.0)
            };
            this.Buttn_StopLossSteps = down10;
            Grid.SetColumn(this.Buttn_StopLossSteps, 1);
            Grid.SetRow(this.Buttn_StopLossSteps, 9);
            this.Grid_Trading.Children.Add(this.Buttn_StopLossSteps);
            IntegerUpDown down11 = new IntegerUpDown {
                Name = "Buttn_TakeProfitSteps",
                Tag = "Buttn_TakeProfitSteps",
                Background = Brushes.Transparent,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Center,
                TextAlignment = TextAlignment.Center,
                Minimum = 0,
                Maximum = 0x1869f,
                Value = 0,
                Margin = new Thickness(16.0, 0.0, 16.0, 0.0)
            };
            this.Buttn_TakeProfitSteps = down11;
            Grid.SetColumn(this.Buttn_TakeProfitSteps, 1);
            Grid.SetRow(this.Buttn_TakeProfitSteps, 10);
            this.Grid_Trading.Children.Add(this.Buttn_TakeProfitSteps);
            ColorPicker picker = new ColorPicker {
                Name = "Buttn_AsksColor",
                Tag = "Buttn_AsksColor",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(16.0, 0.0, 16.0, 0.0)
            };
            this.Buttn_AsksColor = picker;
            Grid.SetColumn(this.Buttn_AsksColor, 1);
            Grid.SetRow(this.Buttn_AsksColor, 0);
            this.Grid_DOM.Children.Add(this.Buttn_AsksColor);
            ColorPicker picker2 = new ColorPicker {
                Name = "Buttn_BidsColor",
                Tag = "Buttn_BidsColor",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(16.0, 0.0, 16.0, 0.0)
            };
            this.Buttn_BidsColor = picker2;
            Grid.SetColumn(this.Buttn_BidsColor, 1);
            Grid.SetRow(this.Buttn_BidsColor, 1);
            this.Grid_DOM.Children.Add(this.Buttn_BidsColor);
            IntegerUpDown down12 = new IntegerUpDown {
                Name = "Buttn_VolumesFilledAt",
                Tag = "Buttn_VolumesFilledAt",
                Background = Brushes.Transparent,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Center,
                TextAlignment = TextAlignment.Center,
                Minimum = 1,
                Maximum = 0x3b9ac9ff,
                Value = 0x3e8,
                Margin = new Thickness(16.0, 0.0, 16.0, 0.0)
            };
            this.Buttn_VolumesFilledAt = down12;
            Grid.SetColumn(this.Buttn_VolumesFilledAt, 1);
            Grid.SetRow(this.Buttn_VolumesFilledAt, 2);
            this.Grid_DOM.Children.Add(this.Buttn_VolumesFilledAt);
            IntegerUpDown down13 = new IntegerUpDown {
                Name = "Buttn_RenewSpeed_DOM",
                Tag = "Buttn_RenewSpeed_DOM",
                Background = Brushes.Transparent,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Center,
                TextAlignment = TextAlignment.Center,
                Minimum = 1,
                Maximum = 50,
                Value = 50,
                Margin = new Thickness(16.0, 0.0, 16.0, 0.0),
                Visibility = Visibility.Hidden
            };
            this.Buttn_RenewSpeed_DOM = down13;
            Grid.SetColumn(this.Buttn_RenewSpeed_DOM, 1);
            Grid.SetRow(this.Buttn_RenewSpeed_DOM, 5);
            this.Grid_DOM.Children.Add(this.Buttn_RenewSpeed_DOM);
            IntegerUpDown down14 = new IntegerUpDown {
                Name = "Buttn_StringHeight",
                Tag = "Buttn_StringHeight",
                Background = Brushes.Transparent,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Center,
                TextAlignment = TextAlignment.Center,
                Minimum = 11,
                Maximum = 0x11,
                Value = 13,
                Margin = new Thickness(16.0, 0.0, 16.0, 0.0)
            };
            this.Buttn_StringHeight = down14;
            Grid.SetColumn(this.Buttn_StringHeight, 1);
            Grid.SetRow(this.Buttn_StringHeight, 4);
            this.Grid_DOM.Children.Add(this.Buttn_StringHeight);
            IntegerUpDown down15 = new IntegerUpDown {
                Name = "Buttn_ClustersFilledAt",
                Tag = "Buttn_ClustersFilledAt",
                Background = Brushes.Transparent,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Center,
                TextAlignment = TextAlignment.Center,
                Minimum = 1,
                Maximum = 0x3b9ac9ff,
                Value = 0x3e8,
                Margin = new Thickness(16.0, 0.0, 16.0, 0.0)
            };
            this.Buttn_ClustersFilledAt = down15;
            Grid.SetColumn(this.Buttn_ClustersFilledAt, 1);
            Grid.SetRow(this.Buttn_ClustersFilledAt, 0);
            this.Grid_Clusters.Children.Add(this.Buttn_ClustersFilledAt);
            IntegerUpDown down16 = new IntegerUpDown {
                Name = "Buttn_PercentsForColorGradient",
                Tag = "Buttn_PercentsForColorGradient",
                Background = Brushes.Transparent,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Center,
                TextAlignment = TextAlignment.Center,
                Minimum = 1,
                Maximum = 100,
                Value = 100,
                Visibility = Visibility.Collapsed,
                Margin = new Thickness(16.0, 0.0, 16.0, 0.0)
            };
            this.Buttn_PercentsForColorGradient = down16;
            Grid.SetColumn(this.Buttn_PercentsForColorGradient, 1);
            Grid.SetRow(this.Buttn_PercentsForColorGradient, 4);
            this.Grid_Clusters.Children.Add(this.Buttn_PercentsForColorGradient);
            IntegerUpDown down17 = new IntegerUpDown {
                Name = "Buttn_ShowTicksFrom",
                Tag = "Buttn_ShowTicksFrom",
                Background = Brushes.Transparent,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Center,
                TextAlignment = TextAlignment.Center,
                Minimum = 1,
                Maximum = 0x3b9ac9ff,
                Value = 1,
                Margin = new Thickness(16.0, 0.0, 16.0, 0.0)
            };
            this.Buttn_ShowTicksFrom = down17;
            Grid.SetColumn(this.Buttn_ShowTicksFrom, 1);
            Grid.SetRow(this.Buttn_ShowTicksFrom, 0);
            this.Grid_Ticks.Children.Add(this.Buttn_ShowTicksFrom);
            IntegerUpDown down18 = new IntegerUpDown {
                Name = "Buttn_FilterTicksFrom",
                Tag = "Buttn_FilterTicksFrom",
                Background = Brushes.Transparent,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Center,
                TextAlignment = TextAlignment.Center,
                Minimum = 1,
                Maximum = 0x3b9ac9ff,
                Value = 1,
                Visibility = Visibility.Collapsed,
                Margin = new Thickness(16.0, 0.0, 16.0, 0.0)
            };
            this.Buttn_FilterTicksFrom = down18;
            Grid.SetColumn(this.Buttn_FilterTicksFrom, 1);
            Grid.SetRow(this.Buttn_FilterTicksFrom, 0);
            this.Grid_Ticks.Children.Add(this.Buttn_FilterTicksFrom);
            IntegerUpDown down19 = new IntegerUpDown {
                Name = "Buttn_TicksWeight",
                Tag = "Buttn_TicksWeight",
                Background = Brushes.Transparent,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Center,
                TextAlignment = TextAlignment.Center,
                Minimum = 1,
                Maximum = 3,
                Value = 1,
                Margin = new Thickness(16.0, 0.0, 16.0, 0.0)
            };
            this.Buttn_TicksWeight = down19;
            Grid.SetColumn(this.Buttn_TicksWeight, 1);
            Grid.SetRow(this.Buttn_TicksWeight, 2);
            this.Grid_Ticks.Children.Add(this.Buttn_TicksWeight);
            IntegerUpDown down20 = new IntegerUpDown {
                Name = "Buttn_RenewSpeed_Ticks",
                Tag = "Buttn_RenewSpeed_Ticks",
                Background = Brushes.Transparent,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Center,
                TextAlignment = TextAlignment.Center,
                Minimum = 1,
                Maximum = 50,
                Value = 50,
                Margin = new Thickness(16.0, 0.0, 16.0, 0.0),
                Visibility = Visibility.Hidden
            };
            this.Buttn_RenewSpeed_Ticks = down20;
            Grid.SetColumn(this.Buttn_RenewSpeed_Ticks, 1);
            Grid.SetRow(this.Buttn_RenewSpeed_Ticks, 6);
            this.Grid_Ticks.Children.Add(this.Buttn_RenewSpeed_Ticks);
        }

        [GeneratedCode("PresentationBuildTasks", "4.0.0.0"), DebuggerNonUserCode]
        public void InitializeComponent()
        {
            if (!this._contentLoaded)
            {
                this._contentLoaded = true;
                Uri resourceLocator = new Uri("/Visualizer;component/settingswindow.xaml", UriKind.Relative);
                Application.LoadComponent(this, resourceLocator);
            }
        }

        private void SettingsWindow_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            base.Hide();
        }

        [GeneratedCode("PresentationBuildTasks", "4.0.0.0"), EditorBrowsable(EditorBrowsableState.Never), DebuggerNonUserCode]
        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    ((SettingsWindow) target).Closing += new CancelEventHandler(this.SettingsWindow_Closing);
                    ((SettingsWindow) target).MouseEnter += new MouseEventHandler(this.Window_MouseEnter_1);
                    ((SettingsWindow) target).MouseLeave += new MouseEventHandler(this.Window_MouseLeave_1);
                    return;

                case 2:
                    this.SettingsTabs = (TabControl) target;
                    return;

                case 3:
                    this.Grid_Trading = (Grid) target;
                    return;

                case 4:
                    this.Buttn_AveragingMethod = (ComboBox) target;
                    return;

                case 5:
                    this.Grid_DOM = (Grid) target;
                    return;

                case 6:
                    this.Buttn_AutoScroll = (Button) target;
                    return;

                case 7:
                    this.Grid_Ticks = (Grid) target;
                    return;

                case 8:
                    this.Buttn_TicksStyle = (ComboBox) target;
                    return;

                case 9:
                    this.Buttn_AddLeader = (Button) target;
                    return;

                case 10:
                    this.Buttn_RemoveLeader = (Button) target;
                    return;

                case 11:
                    this.Buttn_ShowLeadersNames = (Button) target;
                    return;

                case 12:
                    this.Grid_Clusters = (Grid) target;
                    return;

                case 13:
                    this.Buttn_ClustersTF = (ComboBox) target;
                    return;

                case 14:
                    this.Buttn_SaveClusters = (Button) target;
                    return;

                case 15:
                    this.Buttn_ShowClusterPanel = (Button) target;
                    return;

                case 0x10:
                    this.Buttn_ClusterStyleText = (ComboBox) target;
                    return;

                case 0x11:
                    this.Buttn_ClusterStyleColor = (ComboBox) target;
                    return;

                case 0x12:
                    this.Buttn_VertVolumeStyle = (ComboBox) target;
                    return;
            }
            this._contentLoaded = true;
        }

        private void Window_MouseEnter_1(object sender, MouseEventArgs e)
        {
            this.m_dEnterTime = DateTime.Now;
        }

        private void Window_MouseLeave_1(object sender, MouseEventArgs e)
        {
            this.m_dExitTime = DateTime.Now;
            if ((this.m_dEnterTime.Second == this.m_dExitTime.Second) && ((this.m_dExitTime.Millisecond - this.m_dEnterTime.Millisecond) < 30))
            {
                if (this.SettingsTabs.SelectedIndex == 0)
                {
                    this.SettingsTabs.SelectedIndex = 1;
                }
                else if (this.SettingsTabs.SelectedIndex == 1)
                {
                    this.SettingsTabs.SelectedIndex = 0;
                }
            }
        }

        public ColorPicker Buttn_AsksColor { get; set; }

        public IntegerUpDown Buttn_BackAmount { get; set; }

        public ColorPicker Buttn_BidsColor { get; set; }

        public IntegerUpDown Buttn_ClustersFilledAt { get; set; }

        public IntegerUpDown Buttn_DecreaseAmount { get; set; }

        public IntegerUpDown Buttn_FilterTicksFrom { get; set; }

        public IntegerUpDown Buttn_FirstIncreaseAmount { get; set; }

        public IntegerUpDown Buttn_FirstWorkAmount { get; set; }

        public IntegerUpDown Buttn_MaxPosition { get; set; }

        public IntegerUpDown Buttn_PercentsForColorGradient { get; set; }

        public IntegerUpDown Buttn_RenewSpeed_DOM { get; set; }

        public IntegerUpDown Buttn_RenewSpeed_Ticks { get; set; }

        public IntegerUpDown Buttn_SecondIncreaseAmount { get; set; }

        public IntegerUpDown Buttn_SecondWorkAmount { get; set; }

        public IntegerUpDown Buttn_ShowTicksFrom { get; set; }

        public IntegerUpDown Buttn_StopLossSteps { get; set; }

        public IntegerUpDown Buttn_StringHeight { get; set; }

        public IntegerUpDown Buttn_TakeProfitSteps { get; set; }

        public IntegerUpDown Buttn_ThirdWorkAmount { get; set; }

        public IntegerUpDown Buttn_ThrowLimitTo { get; set; }

        public IntegerUpDown Buttn_TicksWeight { get; set; }

        public IntegerUpDown Buttn_VolumesFilledAt { get; set; }
    }
}

