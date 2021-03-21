using System;

public class DistanceHeapTree  {
    static int capacityTree = 10;
int size = 0;

float[] heapTree = new float[capacityTree];

int GetIndexOfParent(int index) { return (index - 1) / 2; }
int GetIndexOfLeftChild(int index) { return index * 2 + 1; }
int GetIndexOfRightChild(int index) { return index * 2 + 2; }

bool HasLeftChild(int index) { return GetIndexOfLeftChild(index) < capacityTree; }
bool HasRightChild(int index) { return GetIndexOfRightChild(index) < capacityTree; }
bool HasParent(int index) { return GetIndexOfParent(index) >= 0; }

float GetLeftChildValue(int index) { return heapTree[GetIndexOfLeftChild(index)]; }
float GetRightChildValue(int index) { return heapTree[GetIndexOfRightChild(index)]; }
float GetParentValue(int index) { return heapTree[GetIndexOfParent(index)]; }

void IncreaseCapacity()
{
    if (size == capacityTree)
    {
        float[] newArray = new float[capacityTree * 2];
        Array.Copy(heapTree, newArray, capacityTree);
        capacityTree *= 2;
        heapTree = newArray;
    }
}

float PeekFirstValue()
{
    if (size == 0)
    {
        throw new Exception();
    }
    return heapTree[0];
}

public float PollNewValue()
{
    if (size == 0)
        throw new Exception();

    float valueOfCurrentIndex = PeekFirstValue();
    heapTree[0] = heapTree[size - 1];

    size--;
    HeapifyDown();
    heapTree[size] = 0;
    return valueOfCurrentIndex;
}

void HeapifyDown()
{
    int index = 0;

    while (HasLeftChild(index))
    {
        int smallerChildIndex = GetIndexOfLeftChild(index);

        if (HasRightChild(index) && GetRightChildValue(index) < GetLeftChildValue(index))
            smallerChildIndex = GetIndexOfRightChild(index);

        if (heapTree[index] < heapTree[smallerChildIndex])
            break;

        Swap(index, smallerChildIndex);
        index = smallerChildIndex;
    }
}

public void Add(int value)
{
    IncreaseCapacity();
    heapTree[size] = value;
    size++;
    HeapifyUp();
}

void Swap(int indexOne, int indexTwo)
{
    float valueCache = heapTree[indexOne];
    heapTree[indexOne] = heapTree[indexTwo];
    heapTree[indexTwo] = valueCache;
}

void HeapifyUp()
{
    int index = size - 1;

    while (HasParent(index) && GetParentValue(index) > heapTree[index])
    {
        Swap(GetIndexOfParent(index), index);
        index = GetIndexOfParent(index);
    }
}

public void PrintTree()
{
    foreach (var item in heapTree)
        if (item != 0)
            Console.WriteLine(item.ToString());

}
    }