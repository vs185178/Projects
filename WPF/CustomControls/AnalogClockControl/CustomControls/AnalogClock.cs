﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace AnalogClockControl.CustomControls
{
    public class AnalogClock: Control
    {
        Line hourHand;
        Line minuteHand;
        Line secondHand;



        public bool ShowSeconds
        {
            get { return (bool)GetValue(ShowSecondsProperty); }
            set { SetValue(ShowSecondsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowSeconds.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowSecondsProperty =
            DependencyProperty.Register("ShowSeconds", typeof(bool), typeof(AnalogClock), new PropertyMetadata(true));



        static AnalogClock()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AnalogClock), new FrameworkPropertyMetadata(typeof(AnalogClock)));
        }

        public override void OnApplyTemplate()
        {
            hourHand = Template.FindName("PART_HourHand", this) as Line;
            minuteHand = Template.FindName("PART_MinuteHand", this) as Line;
            secondHand = Template.FindName("PART_SecondHand", this) as Line;

            Binding showSecondHandBinding = new Binding
            {
                Path = new PropertyPath(nameof(ShowSeconds)),
                Source = this,
                Converter = new BooleanToVisibilityConverter()
            };

            secondHand.SetBinding(VisibilityProperty, showSecondHandBinding);

            UpdateHandAngles();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += (s, e) => UpdateHandAngles();
            timer.Start();
            base.OnApplyTemplate();
        }

        private void UpdateHandAngles()
        {
            hourHand.RenderTransform = new RotateTransform((DateTime.Now.Hour / 12.0) * 360, 0.5, 0.5);
            minuteHand.RenderTransform = new RotateTransform((DateTime.Now.Minute / 60.0) * 360, 0.5, 0.5);
            secondHand.RenderTransform = new RotateTransform((DateTime.Now.Second / 60.0) * 360, 0.5, 0.5);
        }
    }
}
