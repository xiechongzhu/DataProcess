using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DataProcess
{
    public class ChartPointDataSource : List<Point>, INotifyCollectionChanged
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        private int index = 0;
        private int maxCount;
        public ChartPointDataSource(int maxCount)
        {
            this.maxCount = maxCount;
        }

        public void SetMaxCount(int maxCount)
        {
            this.maxCount = maxCount;
        }
        
        public void AddPoints(List<double> values)
        {
            values.ForEach(value =>
            {
                AddPoint(value);
            });
        }

        public int getIndex()
        {
            return this.Count();
        }

        public void AddPoint(double value)
        {
            Add(new Point(index++, value));
            if (Count > maxCount)
            {
                RemoveAt(0);
            }
        }

        public void NotifyDataChanged()
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void ClearPoints()
        {
            index = 0;
            Clear();
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }
}
