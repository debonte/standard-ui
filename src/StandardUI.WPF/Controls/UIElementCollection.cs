using System.Collections;
using System.Collections.Generic;
using System.StandardUI;
using System.Windows;

namespace System.StandardUI.Wpf
{
    public class UIElementCollection : IUIElementCollection
    {
        private Windows.Controls.UIElementCollection _collection;

        public UIElementCollection(FrameworkElement parent)
        {
            _collection = new Windows.Controls.UIElementCollection(parent, parent);
        }

        public IUIElement this[int index]
        {
            get => (IUIElement)_collection[index];
            set => _collection[index] = (Windows.UIElement)value;
        }

        public int Count => _collection.Count;

        public bool IsReadOnly => false;

        public void Add(IUIElement item)
        {
            _collection.Add((Windows.UIElement) item);
        }

        public void Clear() => _collection.Clear();

        public bool Contains(IUIElement item) => _collection.Contains((Windows.UIElement)item);

        public void CopyTo(IUIElement[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<IUIElement> GetEnumerator() => new Enumerator(_collection.GetEnumerator());

        public int IndexOf(IUIElement item) => _collection.IndexOf((Windows.UIElement)item);

        public void Insert(int index, IUIElement item) => _collection.Insert(index, (Windows.UIElement)item);

        public bool Remove(IUIElement item)
        {
            int index = _collection.IndexOf((Windows.UIElement)item);
            if (index == -1)
                return false;
            _collection.RemoveAt(index);
            return true;
        }

        public void RemoveAt(int index) => _collection.RemoveAt(index);

        IEnumerator IEnumerable.GetEnumerator() => _collection.GetEnumerator();

        private class Enumerator : IEnumerator<IUIElement>
        {
            private IEnumerator _enumerator;

            public Enumerator(IEnumerator enumerator)
            {
                _enumerator = enumerator;
            }

#pragma warning disable CS8766 // Nullability of reference types in return type doesn't match implicitly implemented member (possibly because of nullability attributes).
            public IUIElement? Current => (IUIElement?)_enumerator.Current;
#pragma warning restore CS8766 // Nullability of reference types in return type doesn't match implicitly implemented member (possibly because of nullability attributes).

            object? IEnumerator.Current => _enumerator.Current;

            public void Dispose() { }

            public bool MoveNext() => _enumerator.MoveNext();

            public void Reset() => _enumerator.Reset();
        }
    }
}
