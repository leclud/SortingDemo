
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Controls;

public static class SortingAlgorithms
{
    // Bubble Sort
    public static async Task<List<string>> BubbleSort(int[] array, Action<int[], int, int> logStep, int delay, TextBox SortingLog)
    {
        var log = new List<string>();
        SortingLog.AppendText("Начало сортировки пузырьком...");

        for (int i = 0; i < array.Length - 1; i++)
        {
            SortingLog.AppendText($"Проход {i + 1}:");
            for (int j = 0; j < array.Length - i - 1; j++)
            {
                SortingLog.AppendText($"Сравниваем элементы {array[j]} (индекс {j}) и {array[j + 1]} (индекс {j + 1}).");
                if (array[j] > array[j + 1])
                {
                    SortingLog.AppendText($"Элемент {array[j]} больше элемента {array[j + 1]}. Меняем их местами.");
                    (array[j], array[j + 1]) = (array[j + 1], array[j]);
                }
                else
                {
                    SortingLog.AppendText($"Элемент {array[j]} меньше или равен элементу {array[j + 1]}. Оставляем их на месте.");
                }

                logStep(array, j, j + 1);
                await Task.Delay(delay);
            }
        }

        SortingLog.AppendText("Сортировка пузырьком завершена.");
        logStep(array, -1, -1); // Финальная визуализация
        return log;
    }

    // Selection Sort
    public static async Task<List<string>> SelectSort(int[] array, Action<int[], int, int> logStep, int delay, TextBox SortingLog)
    {
        var log = new List<string>();
        SortingLog.AppendText("Начало сортировки выбором...");

        for (int i = 0; i < array.Length - 1; i++)
        {
            int minIndex = i;
            SortingLog.AppendText($"Проход {i + 1}: Ищем минимальный элемент в подмассиве начиная с индекса {i}.");
            for (int j = i + 1; j < array.Length; j++)
            {
                SortingLog.AppendText($"Сравниваем элементы {array[j]} (индекс {j}) и {array[minIndex]} (текущий минимальный, индекс {minIndex}).");
                if (array[j] < array[minIndex])
                {
                    SortingLog.AppendText($"Элемент {array[j]} меньше текущего минимального {array[minIndex]}. Обновляем минимальный элемент.");
                    minIndex = j;
                }
                else
                {
                    SortingLog.AppendText($"Элемент {array[j]} больше или равен текущему минимальному {array[minIndex]}. Ничего не меняем.");
                }

                logStep(array, j, minIndex);
                await Task.Delay(delay);
            }

            if (minIndex != i)
            {
                SortingLog.AppendText($"Меняем местами элементы {array[i]} (индекс {i}) и {array[minIndex]} (индекс {minIndex}).");
                (array[i], array[minIndex]) = (array[minIndex], array[i]);
            }
            else
            {
                SortingLog.AppendText($"Минимальный элемент уже находится на месте. Перестановка не требуется.");
            }

            logStep(array, i, minIndex);
            await Task.Delay(delay);
        }

        SortingLog.AppendText("Сортировка выбором завершена.");
        logStep(array, -1, -1); // Финальная визуализация
        return log;
    }

    // Quick Sort
    public static async Task<List<string>> QuickSort(int[] array, int low, int high, Action<int[], int, int> logStep, int delay, TextBox SortingLog)
    {
        var log = new List<string>();
        if (low < high)
        {
            SortingLog.AppendText($"Работаем с подмассивом от индекса {low} до {high}.");
            int pivotIndex = await Partition(array, low, high, log, logStep, delay, SortingLog);

            SortingLog.AppendText($"Опорный элемент встал на место: индекс {pivotIndex}. Разделяем массив.");
            var leftLog = await QuickSort(array, low, pivotIndex - 1, logStep, delay, SortingLog);
          
            foreach (string i in leftLog)
            {
                SortingLog.AppendText(i);
            }

            var rightLog = await QuickSort(array, pivotIndex + 1, high, logStep, delay, SortingLog);
            foreach (string i in rightLog)
            {
                SortingLog.AppendText(i);
            }
        }
        return log;
    }

    private static async Task<int> Partition(int[] array, int low, int high, List<string> log, Action<int[], int, int> logStep, int delay, TextBox SortingLog)
    {
        int pivot = array[high];
        SortingLog.AppendText($"Выбран опорный элемент {pivot} (индекс {high}).");
        int i = low - 1;

        for (int j = low; j < high; j++)
        {
            SortingLog.AppendText($"Сравниваем элемент {array[j]} (индекс {j}) с опорным элементом {pivot}.");
            if (array[j] <= pivot)
            {
                i++;
                SortingLog.AppendText($"Элемент {array[j]} меньше или равен опорному. Меняем местами с элементом {array[i]} (индекс {i}).");
                (array[i], array[j]) = (array[j], array[i]);
            }
            else
            {
                SortingLog.AppendText($"Элемент {array[j]} больше опорного. Оставляем на месте.");
            }

            logStep(array, i, j);
            await Task.Delay(delay);
        }

        SortingLog.AppendText($"Ставим опорный элемент на место: меняем местами {array[i + 1]} (индекс {i + 1}) и {pivot} (индекс {high}).");
        (array[i + 1], array[high]) = (array[high], array[i + 1]);
        logStep(array, i + 1, high);
        await Task.Delay(delay);

        return i + 1;
    }

    // Heap Sort
    public static async Task<List<string>> HeapSort(int[] array, Action<int[], int, int> logStep, int delay, TextBox SortingLog)
    {
        var log = new List<string>();
        SortingLog.AppendText("Начало пирамидальной сортировки...");

        int n = array.Length;

        for (int i = n / 2 - 1; i >= 0; i--)
        {
            SortingLog.AppendText($"Строим кучу для узла {i}.");
            await Heapify(array, n, i, log, logStep, delay, SortingLog);
        }

        for (int i = n - 1; i > 0; i--)
        {
            SortingLog.AppendText($"Меняем местами корень {array[0]} и последний элемент {array[i]} (индекс {i}).");
            (array[0], array[i]) = (array[i], array[0]);

            logStep(array, 0, i);
            await Task.Delay(delay);

            SortingLog.AppendText($"Восстанавливаем кучу для подмассива длиной {i}.");
            await Heapify(array, i, 0, log, logStep, delay, SortingLog);
        }

        SortingLog.AppendText("Пирамидальная сортировка завершена.");
        logStep(array, -1, -1); // Финальная визуализация
        return log;
    }

    private static async Task Heapify(int[] array, int n, int i, List<string> log, Action<int[], int, int> logStep, int delay, TextBox SortingLog)
    {
        int largest = i;
        int left = 2 * i + 1;
        int right = 2 * i + 2;

        SortingLog.AppendText($"Проверяем узел {i} и его детей: левый = {(left < n ? array[left].ToString() : "нет")}, правый = {(right < n ? array[right].ToString() : "нет")}.");
        if (left < n && array[left] > array[largest])
        {
            SortingLog.AppendText($"Левый ребенок {array[left]} больше текущего узла {array[largest]}. Обновляем.");
            largest = left;
        }

        if (right < n && array[right] > array[largest])
        {
            SortingLog.AppendText($"Правый ребенок {array[right]} больше текущего узла {array[largest]}. Обновляем.");
            largest = right;
        }

        if (largest != i)
        {
            SortingLog.AppendText($"Меняем местами узел {array[i]} и его потомка {array[largest]}.");
            (array[i], array[largest]) = (array[largest], array[i]);
            logStep(array, i, largest);
            await Task.Delay(delay);

            SortingLog.AppendText($"Рекурсивно восстанавливаем кучу для узла {largest}.");
            await Heapify(array, n, largest, log, logStep, delay, SortingLog);
        }
        else
        {
            SortingLog.AppendText($"Узел {i} уже на правильном месте. Ничего не делаем.");
        }
    }
}
