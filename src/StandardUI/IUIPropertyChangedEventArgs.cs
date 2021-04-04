namespace Microsoft.StandardUI
{
    [UIModelObject]
    public interface IUIPropertyChangedEventArgs
    {
#if false
        /// <summary>
        /// Gets the value of the property after the reported change.
        /// </summary>
        public object NewValue { get; }

        /// <summary>
        /// Gets the value of the property before the reported change.
        /// </summary>
        public object OldValue { get; }
#endif

        /// <summary>
        /// Gets the property where the value change occurred.  
    }
}
