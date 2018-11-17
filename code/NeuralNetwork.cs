using System.IO;
using System.Collections.Generic;

public class NeuralNetwork
{
    public List<Layer> layers = new List<Layer>();
    // �w���쐬���A�ׂ̑w�ƌ���������֐�
    public void AddLayer( int numberOfNodes)
    {
        Layer newLayer = new Layer(numberOfNodes);
        if( layers.Count > 0)
        {
            layers[layers.Count - 1].ConnectDensely(newLayer);
        }
        layers.Add(newLayer);
    }
    // ���͑w�Ƀf�[�^����͌�A�o�͑w�Ɍ����Čv�Z���s���֐�
    public void CalcForward(double[] inputdata)
    {
        layers[0].SetInputData(inputdata); // ���͑w�Ƀf�[�^�����

        for( int i = 0; i < layers.Count; i++)
        {
            layers[i].CalcForward();
        }
    }
    // �o�͑w����F�����ʂ��擾����֐�
    public int GetMaxOutput()
    {
        int max = 0;
        double maxValue = 0.0;
        // �o�͑w�̃m�[�h�̒�����ő�l��������
        for ( int i = 0; i < layers[layers.Count-1].nodes.Count; i++ )
        {
            if( layers[layers.Count - 1].nodes[i].value > maxValue )
            {
                max = i; // �ő�l�����m�[�h�̃C���f�b�N�X���L�^
                maxValue = layers[layers.Count - 1].nodes[i].value;
            }
        }
        return max; // �ő�l�����m�[�h�̃C���f�b�N�X��Ԃ�
    }
    public void InitWeight()
    {
        foreach(Layer layer in layers)
        {
            layer.InitWeight();
        }
    }

    public void UpdateWeight( double alpha )
    {
        foreach(Layer layer in layers)
        {
            layer.UpdateWeight(alpha);
        }
    }

    public void CalcError( double[] trainData)
    {
        // �o�͑w�̃m�[�h�����擾
        int numOutput = layers[layers.Count - 1].nodes.Count;
        // �o�͑w�̃m�[�h�̌덷���v�Z
        for ( int i = 0; i < numOutput; i++ )
        {
            layers[layers.Count - 1].nodes[i].CalcError(trainData[i]);
        }
        // �B��w�̃m�[�h�̌덷���v�Z
        for ( int i = layers.Count - 2; i >= 0; i-- )
        {
            foreach( Node node in layers[i].nodes)
            {
                node.CalcError();
            }
        }
    }
    // �j���[�����l�b�g���[�N�̍\���ƑS�̂̏d�݂��t�@�C��[weight.dat]�ɕۑ�����֐�
    public void SaveWeight()
    {
        // �t�@�C�����쐬
        using (FileStream fsWeights = new FileStream(@"weight.dat", FileMode.OpenOrCreate))
        {
            using (BinaryWriter bwWeights = new BinaryWriter(fsWeights))
            {
                bwWeights.Write(layers.Count);// �w�̐�����������
                foreach( Layer layer in layers)
                {
                    // �e�w�̃m�[�h�̐�����������
                    bwWeights.Write(layer.nodes.Count);
                }

                foreach(Layer layer in layers)
                {
                    foreach( Node node in layer.nodes)
                    {
                        foreach(Edge edge in node.inputs)
                        {
                            // �d�݂���������
                            bwWeights.Write(edge.weight);
                        }
                    }
                }
            }
        }
    }
    // �t�@�C��[weight.dat]��ǂݍ���Ńj���[�����l�b�g���[�N���쐬���A�S�̂̏d�݂�ݒ肷��֐�
    public void LoadWeight()
    {
        using (FileStream fsWeights = new FileStream(@"weight.dat", FileMode.Open))
        {
            using (BinaryReader brWeights = new BinaryReader(fsWeights))
            {
                layers = new List<Layer>();
                // �w�̐���ǂݍ���
                int numLayers = brWeights.ReadInt32();
                for(int i = 0; i < numLayers; i++)
                {
                    // �e�w�̃m�[�h�̐���ǂݍ���
                    int numNodes = brWeights.ReadInt32();
                    AddLayer(numNodes); // �w���쐬����
                }
                  
                /* ���̎��_�Ńj���[�����l�b�g���[�N���쐬����� */
                foreach( Layer layer in layers )
                {
                    foreach( LinkedListNode node in layer.nodes )
                    {
                        foreach( Edge edge in node.inputs )
                        {
                            // �d�݂�ǂݍ���Őݒ肷��
                            edge.weight = brWeights.ReadDouble();
                        }
                    }
                }
            }
        }
    }
}


