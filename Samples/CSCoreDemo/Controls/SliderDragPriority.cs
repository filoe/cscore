using System.Windows;
using System.Windows.Controls;

namespace CSCoreDemo.Controls
{
    public class SliderDragPriority : Slider
    {
        public bool AllowValueChange
        {
            get { return (bool)GetValue(AllowValueChangeProperty); }
            set { SetValue(AllowValueChangeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AllowValueChange. This enables
        // animation, styling, binding, etc...
        public static readonly DependencyProperty AllowValueChangeProperty =
            DependencyProperty.Register("AllowValueChange", typeof(bool), typeof(SliderDragPriority), new PropertyMetadata(true));

        protected override void OnThumbDragStarted(System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            base.OnThumbDragStarted(e);
            AllowValueChange = false;
        }

        protected override void OnThumbDragCompleted(System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            AllowValueChange = true;
            base.OnThumbDragCompleted(e);
        }
    }
}