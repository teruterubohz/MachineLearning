using System;
using System.Collections.Generic;

public class Edge
{
    public Node left;
    public Node right;
    public double weight;
}

public class Node
{
    public List<Edge> inputs = new List<Edge>();
    public List<Edge> outputs = new List<Edge>();
    public double inValue;  // ���͒l�̍��v
    public double value;    // �o�͒l
    public double error;    // �덷
    static Random random = new Random();

    // �������֐�
    public double Activation(double val)
    {
        return 1.0 / (1.0 + Math.Exp(-val));
    }

    // �������֐�����������֐�
    public double DActivation(double val)
    {
        return (1.0 - val) * val;
    }

    // �ׂ̃m�[�h�Ɛڑ�����֐�
    public Edge Connect(Node right)
    {
        Edge edge = new Edge();
        edge.left = this;
        edge.right = right;
        right.inputs.Add(edge);
        this.outputs.Add(edge);
        return edge;
    }

    // �o�͒l���v�Z����֐�
    public void CalcForward()
    {
        if (inputs.Count == 0) return;

        inValue = 0.0;
        foreach ( Edge edge in inputs )
        {
            inValue += edge.left.value * edge.weight;
        }
        value = Activation(inValue);
    }

    // ���K���z�����𐶐�����֐�
    public static double GetRandom()
    {
        double r1 = random.NextDouble();
        double r2 = random.NextDouble();
        return (Math.Sqrt(-2.0 * Math.Log(r1)) * Math.Cos(2.0 * Math.PI * r2)) * 0.1;
    }

    // �d�݂𗐐��ŏ���������֐�
    public void InitWeight()
    {
        foreach(Edge edge in inputs)
        {
            edge.weight = GetRandom();
        }
    }

    // (�菇1) �덷�v�Z(�o�͑w)�̊֐�
    public void CalcError(double t)
    {
        error = t - value;
    }

    // (�菇2) �덷�v�Z(�B��w)�̊֐�
    public void CalcError()
    {
        error = 0.0;
        foreach( Edge edge in outputs)
        {
            error += edge.weight * edge.right.error;
        }
    }

    // (�菇3) �d�ݍX�V�֐�
    public void UpdateWeight(double alpha)
    {
        foreach ( Edge edge in inputs )
        {
            // �����l�Z�o
            edge.weight += alpha * error * DActivation(value) * edge.left.value;
        }
    }
}