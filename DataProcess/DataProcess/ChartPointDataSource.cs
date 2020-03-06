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
        private int maxCount;
        public ChartPointDataSource(int maxCount)
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

        public void AddPoint(double value)
        {
            int endX = Count > 0 ? (int)this[Count - 1].Y : -1;
            Add(new Point(++endX, value));
            if (Count > maxCount)
            {
                RemoveRange(0, Count - maxCount);
            }
        }

        public void NotifyDataChanged()
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }
}
