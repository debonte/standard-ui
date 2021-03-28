namespace System.StandardUI.Controls
{
    public interface IControl : IUIElement
    {
        /// <summary>
        /// Retrieves the named element in the instantiated ControlTemplate visual tree.
        /// </summary>
        /// <param name="childName">The name of the element to find.</param>
        /// <returns>The named element from the template, if the element is found. Can return null if no
        /// element with name childName was found in the template.</returns>
        IUIPropertyObject? GetTemplateChild(string childName);
    }
}
