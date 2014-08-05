using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

/// <summary>
/// Extension methods for INotifyPropertyChangedExtension
/// </summary>
public static class INotifyPropertyChangedExtension
{
    #if NETFX_45

    /// <summary>
    /// Checks if a property already matches a desired value.  Sets the property and
    /// notifies listeners only when necessary.
    /// </summary>
    /// <param name="sender">Reference to the INotifyPropertyChanged object</param>
    /// <param name="handler">Represents the method that will handle the INotifyPropertyChanged.PropertyChanged event raised when a property is changed on a component./// </param>
    /// <param name="propertyName">Property name to notify.</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
    public static void Notify(this INotifyPropertyChanged sender, PropertyChangedEventHandler handler, [CallerMemberName] string propertyName = null)
    {
        if (handler != null)
        {
            PropertyChangedEventArgs args = new PropertyChangedEventArgs(propertyName);
            handler(sender, args);
        }        
    }

    /// <summary>
    /// Checks if a property already matches a desired value.  Sets the property and
    /// notifies listeners only when necessary.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sender">Reference to the INotifyPropertyChanged object</param>
    /// <param name="handler">Represents the method that will handle the INotifyPropertyChanged.PropertyChanged event raised when a property is changed on a component./// </param>
    /// <param name="field">Reference to the property field to set.</param>
    /// <param name="value">Value to set in the specified property field.</param>
    /// <param name="propertyName">Property name to notify.</param>
    /// <returns>True if the value was changed, false if the existing value matched the desired value.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "2#"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
    public static bool SetAndNotify<T>(this INotifyPropertyChanged sender, PropertyChangedEventHandler handler, ref T field, T value, [CallerMemberName] string propertyName = null)
    {
        if (object.Equals(field, value)) return false;

        field = value;
        sender.Notify(handler, propertyName);
        return true;
    }

#else

    /// <summary>
    /// Checks if a property already matches a desired value.  Sets the property and
    /// notifies listeners only when necessary.
    /// </summary>
    /// <param name="sender">Reference to the INotifyPropertyChanged object</param>
    /// <param name="handler">Represents the method that will handle the INotifyPropertyChanged.PropertyChanged event raised when a property is changed on a component./// </param>
    /// <param name="propertyName">Property name to notify.</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
    public static void Notify(this INotifyPropertyChanged sender, PropertyChangedEventHandler handler, string propertyName)
    {
        if (handler != null)
        {
            PropertyChangedEventArgs args = new PropertyChangedEventArgs(propertyName);
            handler(sender, args);
        }        
    }

    /// <summary>
    /// Checks if a property already matches a desired value.  Sets the property and
    /// notifies listeners only when necessary.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sender">Reference to the INotifyPropertyChanged object</param>
    /// <param name="handler">Represents the method that will handle the INotifyPropertyChanged.PropertyChanged event raised when a property is changed on a component./// </param>
    /// <param name="field">Reference to the property field to set.</param>
    /// <param name="value">Value to set in the specified property field.</param>
    /// <param name="propertyName">Property name to notify.</param>
    /// <returns>True if the value was changed, false if the existing value matched the desired value.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "2#")]
    public static bool SetAndNotify<T>(this INotifyPropertyChanged sender, PropertyChangedEventHandler handler, ref T field, T value, string propertyName)
    {
        if (object.Equals(field, value)) return false;

        field = value;
        sender.Notify(handler, propertyName);
        return true;
    }

    #endif
}

