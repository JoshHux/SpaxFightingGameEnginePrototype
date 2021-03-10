using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputReader
{
    public SingleCircularList<InputInfo> inputRecord;

    public InputReader()
    {
        inputRecord = new SingleCircularList<InputInfo>(8);
    }
}

public class SingleCircularList<T>
{
    public int length;

    private LinkedNode<T> head;
    private LinkedNode<T> tail;

    public SingleCircularList(int size)
    {
        length = size;

        head = new LinkedNode<T>();
        tail = head;

        for (int i = 0; i < length; i++)
        {
            tail.SetNext(new LinkedNode<T>());
            tail = tail.GetNext();
        }

        tail.SetNext(head);
    }

    public void SetHeadInfo(T newInfo)
    {
        head.SetInfo(newInfo);
    }

    public void SetTailInfo(T newInfo)
    {
        tail.SetInfo(newInfo);
    }

    public T GetHeadInfo()
    {
        return head.GetInfo();
    }

    public T GetTailInfo()
    {
        return tail.GetInfo();
    }

    public T GetAtIndex(int index)
    {
        if (index > length)
        {
            index = index % length;
        }

        return RecurGetInfo(head, index);
    }

    public void Shift()
    {
        head = head.GetNext();

        tail = tail.GetNext();
    }

    private T RecurGetInfo(LinkedNode<T> node, int index)
    {
        if (index <= 0)
        {
            return node.GetInfo();
        }
        return RecurGetInfo(node.GetNext(), index - 1);
    }


    public T[] ToArray()
    {
        T[] ret = new T[length];
        LinkedNode<T> temp = head;

        for (int i = 0; i < length; i++)
        {
            ret[i] = temp.GetInfo();
            temp = temp.GetNext();
        }

        return ret;
    }

}

public class LinkedNode<T>
{
    private T info;
    private LinkedNode<T> next;

    public LinkedNode()
    {

    }

    public void SetNext(LinkedNode<T> newNext)
    {
        next = newNext;
    }

    public void SetInfo(T newInfo)
    {
        info = newInfo;
    }

    public LinkedNode<T> GetNext()
    {
        return next;
    }

    public T GetInfo()
    {
        return info;
    }
}

public class InputInfo
{
    public ControllerInput input;
    public int heldframes;
}

public class ControllerInput
{
    public int direction;
    public FaceButtons faceInput;

    public void AddFaceButton(FaceButtons buttons)
    {
        //should add only the necessary bits to the input using an "or" assignment
        faceInput |= buttons;
    }
    public void RemoveFaceButton(FaceButtons buttons)
    {
        //should remove only the matching bits to the input using a "xor" assignment
        faceInput ^= buttons;
    }

    public void ResetFaceButtons()
    {
        faceInput = FaceButtons.None;
    }
}

public enum FaceButtons
{
    None = 0b_0000_0000,
    LightAttack = 0b_0000_0001,
    MediumAttack = 0b_0000_0010,
    HeavyAttack = 0b_0000_0100,
    Jump = 0b_0000_1000,
    Sheld = 0b_0001_0000,
    Grab = 0b_0010_0000
}

