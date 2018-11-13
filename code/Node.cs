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
    public double inValue;  // 入力値の合計
    public double value;    // 出力値
    public double error;    // 誤差
    static Random random = new Random();

    // 活性化関数
    public double Activation(double val)
    {
        return 1.0 / (1.0 + Math.Exp(-val));
    }

    // 活性化関数を微分した関数
    public double DActivation(double val)
    {
        return (1.0 - val) * val;
    }

    // 隣のノードと接続する関数
    public Edge Connect(Node right)
    {
        Edge edge = new Edge();
        edge.left = this;
        edge.right = right;
        right.inputs.Add(edge);
        this.outputs.Add(edge);
        return edge;
    }

    // 出力値を計算する関数
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

    // 正規分布乱数を生成する関数
    public static double GetRandom()
    {
        double r1 = random.NextDouble();
        double r2 = random.NextDouble();
        return (Math.Sqrt(-2.0 * Math.Log(r1)) * Math.Cos(2.0 * Math.PI * r2)) * 0.1;
    }

    // 重みを乱数で初期化する関数
    public void InitWeight()
    {
        foreach(Edge edge in inputs)
        {
            edge.weight = GetRandom();
        }
    }

    // (手順1) 誤差計算(出力層)の関数
    public void CalcError(double t)
    {
        error = t - value;
    }

    // (手順2) 誤差計算(隠れ層)の関数
    public void CalcError()
    {
        error = 0.0;
        foreach( Edge edge in outputs)
        {
            error += edge.weight * edge.right.error;
        }
    }

    // (手順3) 重み更新関数
    public void UpdateWeight(double alpha)
    {
        foreach ( Edge edge in inputs )
        {
            // 調整値算出
            edge.weight += alpha * error * DActivation(value) * edge.left.value;
        }
    }
}