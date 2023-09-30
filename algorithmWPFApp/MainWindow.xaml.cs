using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace algorithmWPFApp
{
    public partial class MainWindow : Window
    {
        private Stopwatch stopwatch = new Stopwatch();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SortButton_Click(object sender, RoutedEventArgs e)
        {
            string input = InputTextBox.Text;
            List<int> numbers = input.Split(',').Select(int.Parse).ToList();
            string algorithm = ((ComboBoxItem)AlgorithmComboBox.SelectedItem).Content.ToString();

            List<ListViewItem> steps = new List<ListViewItem>();
            stopwatch.Restart(); // Start measuring time

            if (algorithm == "Bubble Sort")
            {
                BubbleSort(numbers, steps);
            }
            else if (algorithm == "Selection Sort")
            {
                SelectionSort(numbers, steps);
            }
            else if (algorithm == "Merge Sort")
            {
                MergeSort(numbers, steps);
            }
            else if (algorithm == "Quick Sort")
            {
                QuickSort(numbers, 0, numbers.Count - 1, steps);
            }

            stopwatch.Stop(); // Stop measuring time

            ResultListView.ItemsSource = steps;
            DisplaySortedArray(numbers);
            DisplayStepCount(steps.Count);
            DisplayTimeTaken(stopwatch.ElapsedMilliseconds);
        }

        private void DisplaySortedArray(List<int> arr)
        {
            OutputTextBlock.Text = "Sorted Array: " + string.Join(", ", arr);
        }

        private void DisplayStepCount(int stepCount)
        {
            StepCountTextBlock.Text = "Step Count: " + stepCount;
        }

        private void DisplayTimeTaken(long milliseconds)
        {
            TimeTakenTextBlock.Text = "Time Taken: " + milliseconds.ToString("F3") + " ms";
        }


        private void MergeSort(List<int> arr, List<ListViewItem> steps)
        {
            int n = arr.Count;
            if (n < 2) return;

            int mid = n / 2;
            List<int> left = arr.GetRange(0, mid);
            List<int> right = arr.GetRange(mid, n - mid);

            MergeSort(left, steps);
            MergeSort(right, steps);

            Merge(arr, left, right, steps);
        }

        private void Merge(List<int> arr, List<int> left, List<int> right, List<ListViewItem> steps)
        {
            int i = 0, j = 0, k = 0;
            int leftCount = left.Count, rightCount = right.Count;

            while (i < leftCount && j < rightCount)
            {
                if (left[i] <= right[j])
                {
                    arr[k++] = left[i++];
                }
                else
                {
                    arr[k++] = right[j++];
                }
                steps.Add(CreateListViewItem(steps.Count + 1, arr));
            }

            while (i < leftCount)
            {
                arr[k++] = left[i++];
                steps.Add(CreateListViewItem(steps.Count + 1, arr));
            }

            while (j < rightCount)
            {
                arr[k++] = right[j++];
                steps.Add(CreateListViewItem(steps.Count + 1, arr));
            }
        }


        private void QuickSort(List<int> arr, int low, int high, List<ListViewItem> steps)
        {
            if (low < high)
            {
                int pivotIndex = Partition(arr, low, high, steps);
                QuickSort(arr, low, pivotIndex - 1, steps);
                QuickSort(arr, pivotIndex + 1, high, steps);
            }
        }

        private int Partition(List<int> arr, int low, int high, List<ListViewItem> steps)
        {
            int pivot = arr[high];
            int i = low - 1;

            for (int j = low; j < high; j++)
            {
                if (arr[j] < pivot)
                {
                    i++;
                    Swap(arr, i, j);
                    steps.Add(CreateListViewItem(steps.Count + 1, arr));
                }
            }

            Swap(arr, i + 1, high);
            steps.Add(CreateListViewItem(steps.Count + 1, arr));
            return i + 1;
        }

        private void BubbleSort(List<int> arr, List<ListViewItem> steps)
        {
            int n = arr.Count;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    if (arr[j] > arr[j + 1])
                    {
                        Swap(arr, j, j + 1);
                        steps.Add(CreateListViewItem(steps.Count + 1, arr));
                    }
                }
            }
        }

        private void SelectionSort(List<int> arr, List<ListViewItem> steps)
        {
            int n = arr.Count;
            for (int i = 0; i < n - 1; i++)
            {
                int minIndex = i;
                for (int j = i + 1; j < n; j++)
                {
                    if (arr[j] < arr[minIndex])
                    {
                        minIndex = j;
                    }
                }
                Swap(arr, i, minIndex);
                steps.Add(CreateListViewItem(steps.Count + 1, arr));
            }
        }

        private void Swap(List<int> arr, int i, int j)
        {
            int temp = arr[i];
            arr[i] = arr[j];
            arr[j] = temp;
        }

        private ListViewItem CreateListViewItem(int step, List<int> arr)
        {
            ListViewItem item = new ListViewItem();
            item.Content = new { Step = step, Array = string.Join(", ", arr) };
            return item;
        }
    }
}
