using System;
//Heap data structure
public class PathHeap<T> where T : IHeapItem<T>
{
    readonly T[] items;
    int currentItemCount;
     
    public PathHeap(int maxHeapSize)
    {
        items = new T[maxHeapSize];
    }
    //Add an item to the heap
    public void Add(T heapItem)
    {
        heapItem.HeapIndex = currentItemCount;
        items[currentItemCount] = heapItem;
        SortUp(heapItem);
        currentItemCount++;
    }
    // 
    public T RemoveFirst()
    {
        T firstItem = items[0];
        currentItemCount--;
        items[0] = items[currentItemCount];
        items[0].HeapIndex = 0;
        SortDown(items[0]);
        return firstItem;
    }
    //Update the position of an item in the heap
    public void UpdateItem(T heapItem)
    {
        SortUp(heapItem);
    }
    //Returns the amount of items in the heap
    public int Count
    {
        get
        {
            return currentItemCount;
        }
    }
    //Check if the heap contains an item
    public bool Contains(T heapItem)
    {
        return Equals(items[heapItem.HeapIndex], heapItem);
    }
    //Sort the heap down
    void SortDown(T heapItem)
    {
        while (true)
        {
            int childIndexLeft = heapItem.HeapIndex * 2 + 1;
            int childIndexRight = heapItem.HeapIndex * 2 + 2;
            //Checks if left child is in the heap
            if (childIndexLeft < currentItemCount)
            {
                //Then checks if right child is in the heap
                int swapIndex = childIndexLeft;
                if (childIndexRight < currentItemCount)
                {
                    //Check which child is greater
                    if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0)
                    {
                        swapIndex = childIndexRight;
                    }
                }
                //Check if the item is less than the child
                if (heapItem.CompareTo(items[swapIndex]) < 0)
                {
                    Swap(heapItem, items[swapIndex]);
                }
                else
                {
                    return;
                }

            }
            else
            {
                return;
            }

        }
    }
    //Sort the heap up 
    void SortUp(T heapItem)
    {
        int parentIndex = (heapItem.HeapIndex - 1) / 2;

        while (true)
        {
            T parentItem = items[parentIndex];
            if (heapItem.CompareTo(parentItem) > 0)
            {
                Swap(heapItem, parentItem);
            }
            else
            {
                break;
            }

            parentIndex = (heapItem.HeapIndex - 1) / 2;
        }
    }
    
    //Swap two items in the heap
    void Swap(T heapItemA, T heapItemB)
    {
        items[heapItemA.HeapIndex] = heapItemB;
        items[heapItemB.HeapIndex] = heapItemA;
        (heapItemB.HeapIndex, heapItemA.HeapIndex) = (heapItemA.HeapIndex, heapItemB.HeapIndex);
    }
}
// Interface for the heap
public interface IHeapItem<T> : IComparable<T>
{
    int HeapIndex
    {
        get;
        set;
    }
}