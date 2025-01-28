using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Calendar = System.Windows.Controls.Calendar;
using CalendarMode = System.Windows.Controls.CalendarMode;
using CalendarModeChangedEventArgs = System.Windows.Controls.CalendarModeChangedEventArgs;
using DatePicker = System.Windows.Controls.DatePicker;

namespace WPF.Behaviors
{

    public class DatePickerCalendar
    {
        #region Attached Dependency Property
        public static bool GetIsMonthYear(DatePicker obj)
        {
            return (bool)obj.GetValue(IsMonthYearProperty);
        }

        public static void SetIsMonthYear(DatePicker obj, bool value)
        {
            obj.SetValue(IsMonthYearProperty, value);
        }

        public static readonly DependencyProperty IsMonthYearProperty =
            DependencyProperty.RegisterAttached("IsMonthYear", typeof(bool), typeof(DatePickerCalendar),
                                                new PropertyMetadata(OnIsMonthYearChanged));
        #endregion

        /// <summary>
        /// Se ejecuta cuando se cambia el valor de la porpiedad "IsMonthYear"/>
        /// </summary>
        /// <param name="dobj">Objeto que cambio la propiedad</param>
        /// <param name="e">Propiedad IsMonthYear</param>
        private static void OnIsMonthYearChanged(DependencyObject dobj, DependencyPropertyChangedEventArgs e)
        {
            DatePicker? datePicker = (DatePicker)dobj;

            _ = Application.Current.Dispatcher
                .BeginInvoke(DispatcherPriority.Loaded,
                             new Action<DatePicker, DependencyPropertyChangedEventArgs>(SetCalendarEventHandlers),
                             datePicker, e);
        }

        private static void SetCalendarEventHandlers(DatePicker datePicker, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == e.OldValue) { return; }

            if ((bool)e.NewValue)   //isMonthYear==True
            {
                datePicker.CalendarOpened += DatePickerOnCalendarOpened;
                datePicker.CalendarClosed += DatePickerOnCalendarClosed;
            }
            else
            {
                datePicker.CalendarOpened -= DatePickerOnCalendarOpened;
                datePicker.CalendarClosed -= DatePickerOnCalendarClosed;
            }
        }

        /// <summary>
        /// Se ejecuta al abrir el drop-down del calendario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="routedEventArgs"></param>
        private static void DatePickerOnCalendarOpened(object sender, RoutedEventArgs routedEventArgs)
        {
            DatePicker datePicker = (DatePicker)sender;
            Calendar calendar = GetDatePickerCalendar(datePicker);
            calendar.DisplayMode = CalendarMode.Year;

            calendar.DisplayModeChanged += CalendarOnDisplayModeChanged;
        }

        /// <summary>
        /// Se ejecuta al cerrar el drop-down del calendario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="routedEventArgs"></param>
        private static void DatePickerOnCalendarClosed(object sender, RoutedEventArgs routedEventArgs)
        {
            DatePicker datePicker = (DatePicker)sender;
            Calendar calendar = GetDatePickerCalendar(datePicker);
            datePicker.SelectedDate = calendar.SelectedDate;

            calendar.DisplayModeChanged -= CalendarOnDisplayModeChanged;
        }

        /// <summary>
        /// Se ejecuta cuando el "DisplayMode" del <see cref="Calendar"/> cambia
        /// Si el calendario esta en modo MES (para seleccionar el dia) se elige el ultimo dia del mes
        /// y se cierra el Calendario DropDown
        /// </summary>
        /// <param name="sender">El Objeto Calendar</param>
        /// <param name="e"></param>
        private static void CalendarOnDisplayModeChanged(object? sender, CalendarModeChangedEventArgs e)
        {
            Calendar? calendar = sender as Calendar;
            if (calendar?.DisplayMode == CalendarMode.Month)
            {
                calendar.SelectedDate = GetSelectedCalendarDate(calendar.DisplayDate);

                DatePicker datePicker = GetCalendarsDatePicker(calendar);
                datePicker.IsDropDownOpen = false;
            }
        }

        /// <summary>
        /// Obtiene el <see cref="Calendar"/> del <see cref="DatePicker"/> dado
        /// </summary>
        /// <param name="dp">DatePicker del que se quiere obtener su Calendar</param>
        /// <returns>El objeto Calendar del DatePicker dado</returns>
        private static Calendar GetDatePickerCalendar(DatePicker dp)
        {
            Popup popup = (Popup)dp.Template.FindName("PART_Popup", dp);
            return (Calendar)popup.Child;
        }

        /// <summary>
        /// Método recursivo para obtener el objeto padre <see cref="DatePicker"/> del objeto hijo dado
        /// </summary>
        /// <param name="child">Objeto Hijo</param>
        /// <returns>El <see cref="DatePicker"/> donde se encuentra el objeto Hijo</returns>
        private static DatePicker GetCalendarsDatePicker(FrameworkElement child)
        {
            FrameworkElement parent = (FrameworkElement)child.Parent;
            return parent.Name == "PART_Root" ? (DatePicker)parent.TemplatedParent : GetCalendarsDatePicker(parent);
        }

        /// <summary>
        /// Convierte la fecha dada en el ultimo dia del mes
        /// </summary>
        /// <param name="selectedDate">Fecha a convertir</param>
        /// <returns></returns>
        private static DateTime? GetSelectedCalendarDate(DateTime? selectedDate)
        {
            return !selectedDate.HasValue
                ? null
                : new DateTime(selectedDate.Value.Year, selectedDate.Value.Month, DateTime.DaysInMonth(selectedDate.Value.Year, selectedDate.Value.Month));
        }
    }
}