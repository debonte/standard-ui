namespace Microsoft.StandardUI
{
    [UIModelObject]
    public interface IDependencyObject
    {
        /// <summary>
        /// Returns the current effective value of a dependency property from an IDependencyObject.
        /// </summary>
        /// <param name="dp">The DependencyProperty identifier of the property for which to retrieve the value.</param>
        /// <returns>Returns the current effective value.</returns>
        public object GetValue(IDependencyProperty dp);

        /// <summary>
        /// Returns the local value of a dependency property, if a local value is set.
        /// </summary>
        /// <param name="dp">The IDependencyProperty identifier of the property for which to retrieve the local value.</param>
        /// <returns>Returns the local value, or returns the sentinel value UnsetValue if no local value is set.</returns>
        public object ReadLocalValue(IDependencyProperty dp);

        /// <summary>
        /// Sets the local value of a dependency property on an IDependencyObject.
        /// </summary>
        /// <param name="dp">The identifier of the dependency property to set.</param>
        /// <param name="value">The new local value.</param>
        public void SetValue(IDependencyProperty dp, object value);
    }
}
