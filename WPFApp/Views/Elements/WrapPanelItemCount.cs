using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WPFApp.Views
{
    public class WrapPanelItemsCount : WrapPanel
    {
        public static readonly DependencyProperty MaxItemsCountProperty =
            DependencyProperty.Register(
                "RowsOrColumnsCount",
                typeof(int),
                typeof(WrapPanelItemsCount),
                new PropertyMetadata(int.MaxValue));

        public int MaxItemsCount
        {
            get { return (int)GetValue(MaxItemsCountProperty); }
            set { SetValue(MaxItemsCountProperty, Math.Max(value, 1)); }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (Children.Count > 0)
            {
                Size newAvailableSize;

                if (Orientation == Orientation.Horizontal)
                {
                    var suitableWidth = EstimateSuitableRowOrColumnLength(Children.Cast<UIElement>(),
                                                                            true,
                                                                            availableSize,
                                                                            MaxItemsCount);

                    newAvailableSize =
                        double.IsNaN(suitableWidth) || suitableWidth <= 0
                            ? availableSize
                            : new Size(Math.Min(suitableWidth, availableSize.Width), availableSize.Height);
                }
                else //Orientation == Orientation.Vertical
                {
                    var suitableHeigth = EstimateSuitableRowOrColumnLength(Children.Cast<UIElement>(),
                                                                            false,
                                                                            availableSize,
                                                                            MaxItemsCount);
                    newAvailableSize =
                        double.IsNaN(suitableHeigth) || suitableHeigth <= 0
                            ? availableSize
                            : new Size(availableSize.Width, Math.Min(suitableHeigth, availableSize.Height));
                }

                return base.MeasureOverride(newAvailableSize);
            }
            else
            {
                return base.MeasureOverride(availableSize);
            }
        }

        private double EstimateSuitableRowOrColumnLength(IEnumerable<UIElement> elements,
                                                            bool trueRowsFalseColumns,
                                                            Size availableSize,
                                                            int rowsOrColumnsCount)
        {
            var elementsList = elements.ToList();

            var desiredLengths = elementsList.Select(el => DesiredLength(el, availableSize, trueRowsFalseColumns)).ToList();

            var maxLength = desiredLengths.Where(length => !double.IsNaN(length)).Concat(new[] { 0.0 }).Max();

            if (maxLength <= 0.0)
            {
                return double.NaN;
            }

            var desiredLengthsRepaired = desiredLengths.Select(length => double.IsNaN(length) ? maxLength : length).ToList();

            var totalDesiredLength = desiredLengthsRepaired.Sum();

            var maxCount = Math.Min(rowsOrColumnsCount, elementsList.Count);

            var suitableRowOrColumnLength = totalDesiredLength / maxCount;

            double nextLengthIncrement;

            while (CountRowsOrColumnsNumber(desiredLengthsRepaired, suitableRowOrColumnLength, out nextLengthIncrement) > maxCount)
            {
                suitableRowOrColumnLength += nextLengthIncrement;
            }

            suitableRowOrColumnLength = Math.Max(suitableRowOrColumnLength, desiredLengthsRepaired.Max());

            return suitableRowOrColumnLength;
        }

        private int CountRowsOrColumnsNumber(List<double> desiredLengths, double rowOrColumnLengthLimit, out double nextLengthIncrement)
        {
            int rowOrColumnCount = 1;
            double currentCumulativeLength = 0;
            bool nextNewRowOrColumn = false;

            var minimalIncrement = double.MaxValue;

            foreach (var desiredLength in desiredLengths)
            {
                if (nextNewRowOrColumn)
                {
                    rowOrColumnCount++;
                    currentCumulativeLength = 0;
                    nextNewRowOrColumn = false;
                }

                if (currentCumulativeLength + desiredLength > rowOrColumnLengthLimit)
                {
                    minimalIncrement = Math.Min(minimalIncrement,
                                                currentCumulativeLength + desiredLength - rowOrColumnLengthLimit);

                    if (currentCumulativeLength == 0)
                    {
                        nextNewRowOrColumn = true;
                        currentCumulativeLength = 0;
                    }
                    else
                    {
                        rowOrColumnCount++;
                        currentCumulativeLength = desiredLength;
                    }
                }
                else
                {
                    currentCumulativeLength += desiredLength;
                }
            }

            nextLengthIncrement = minimalIncrement != double.MaxValue ? minimalIncrement : 1;

            return rowOrColumnCount;
        }

        private double DesiredLength(UIElement el, Size availableSize, bool trueRowsFalseColumns)
        {
            el.Measure(availableSize);
            Size next = el.DesiredSize;

            var length = trueRowsFalseColumns ? next.Width : next.Height;

            if (Double.IsInfinity(length) ||
                Double.IsNaN(length))
            {
                return Double.NaN;
            }
            else
            {
                return length;
            }
        }
    }
}
