using System;
using System.Collections.Generic;

public class Layer
{
    public List<Node> nodes = new List<Node>();
    // �R���X�g���N�^
    public Layer(int numNodes)
    {
        for (int i = 0; i < numNodes; i++)
        {
            Node node = new Node();
            nodes.Add(node);
        }
    }

    // �m�[�h�̑S�����̂��߂̊֐�
    public void ConnectDensely(Layer reightLayer)
    {
        foreach (Node node in nodes)
        {
            foreach (Node nextNode in reightLayer.nodes)
            {
                node.Connect(nextNode);
            }
        }
    }

    public void InitWeight()
    {
        foreach (LinkedListNode node in nodes)
        {
            node.InitWeight();
        }
    }
    // �m�[�h�Ƀf�[�^����͂��邽�߂̊֐�
    public void SetInputData(double[] input)
    {
        for( int i = 0; i < nodes.Count; i++ )
        {
            nodes[i].value = input[i];
        }
    }

    public void CalcForward()
    {
        foreach( Node node in nodes )
        {
            node.CalcForward();
        }
    }

    public void UpdateWeight( double alpha )
    {
        foreach( Node node in nodes )
        {
            node.UpdateWeight(alpha);
        }
    }
}
