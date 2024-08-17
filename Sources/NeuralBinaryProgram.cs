using System;

namespace NeuralBinary
{
  class NeuralBinaryProgram
  {
    static void Main(string[] args)
    {
      Console.WriteLine("\nBegin neural network binary classification demo\n");
      Console.WriteLine("Goal is to predict iris species (setosa or versicolor)");
      Console.WriteLine("from sepal length and width and petal length and width");
      Console.WriteLine("\nRaw data looks like:\n");
      Console.WriteLine("[ 0] 5.1, 3.5, 1.4, 0.2, Iris-setosa");
      Console.WriteLine("[ 1] 4.9, 3.0, 1.4, 0.2, Iris-setosa");
      Console.WriteLine(" . . .");
      Console.WriteLine("[49] 5.0, 3.3, 1.4, 0.2, Iris-setosa");
      Console.WriteLine("[50] 7.0, 3.2, 4.7, 1.4, Iris-versicolor");
      Console.WriteLine("[51] 6.4, 3.2, 4.5, 1.5, Iris-versicolor");
      Console.WriteLine(" . . .");
      Console.WriteLine("[99] 5.7, 2.8, 4.1, 1.3, Iris-versicolor");

      // hard-coded; in realistic scenarios you'd read from a text file
      double[][] trainData = new double[80][];
      trainData[0] = new double[] { 4.7, 3.2, 1.3, 0.2, 1, 0 }; // setosa
      trainData[1] = new double[] { 4.6, 3.1, 1.5, 0.2, 1, 0 };
      trainData[2] = new double[] { 5.0, 3.6, 1.4, 0.2, 1, 0 };
      trainData[3] = new double[] { 5.4, 3.9, 1.7, 0.4, 1, 0 };
      trainData[4] = new double[] { 4.6, 3.4, 1.4, 0.3, 1, 0 };
      trainData[5] = new double[] { 5.0, 3.4, 1.5, 0.2, 1, 0 };
      trainData[6] = new double[] { 4.4, 2.9, 1.4, 0.2, 1, 0 };
      trainData[7] = new double[] { 4.9, 3.1, 1.5, 0.1, 1, 0 };
      trainData[8] = new double[] { 5.4, 3.7, 1.5, 0.2, 1, 0 };
      trainData[9] = new double[] { 4.8, 3.4, 1.6, 0.2, 1, 0 };

      trainData[10] = new double[] { 4.8, 3.0, 1.4, 0.1, 1, 0 };
      trainData[11] = new double[] { 4.3, 3.0, 1.1, 0.1, 1, 0 };
      trainData[12] = new double[] { 5.4, 3.9, 1.3, 0.4, 1, 0 };
      trainData[13] = new double[] { 5.1, 3.5, 1.4, 0.3, 1, 0 };
      trainData[14] = new double[] { 5.7, 3.8, 1.7, 0.3, 1, 0 };
      trainData[15] = new double[] { 5.1, 3.8, 1.5, 0.3, 1, 0 };
      trainData[16] = new double[] { 5.4, 3.4, 1.7, 0.2, 1, 0 };
      trainData[17] = new double[] { 5.1, 3.7, 1.5, 0.4, 1, 0 };
      trainData[18] = new double[] { 4.8, 3.4, 1.9, 0.2, 1, 0 };
      trainData[19] = new double[] { 5.0, 3.0, 1.6, 0.2, 1, 0 };

      trainData[20] = new double[] { 5.0, 3.4, 1.6, 0.4, 1, 0 };
      trainData[21] = new double[] { 5.2, 3.5, 1.5, 0.2, 1, 0 };
      trainData[22] = new double[] { 4.8, 3.1, 1.6, 0.2, 1, 0 };
      trainData[23] = new double[] { 5.4, 3.4, 1.5, 0.4, 1, 0 };
      trainData[24] = new double[] { 5.2, 4.1, 1.5, 0.1, 1, 0 };
      trainData[25] = new double[] { 5.5, 4.2, 1.4, 0.2, 1, 0 };
      trainData[26] = new double[] { 4.9, 3.1, 1.5, 0.1, 1, 0 };
      trainData[27] = new double[] { 5.0, 3.2, 1.2, 0.2, 1, 0 };
      trainData[28] = new double[] { 5.5, 3.5, 1.3, 0.2, 1, 0 };
      trainData[29] = new double[] { 4.9, 3.1, 1.5, 0.1, 1, 0 };

      trainData[30] = new double[] { 4.4, 3.0, 1.3, 0.2, 1, 0 };
      trainData[31] = new double[] { 5.1, 3.4, 1.5, 0.2, 1, 0 };
      trainData[32] = new double[] { 5.0, 3.5, 1.3, 0.3, 1, 0 };
      trainData[33] = new double[] { 4.5, 2.3, 1.3, 0.3, 1, 0 };
      trainData[34] = new double[] { 4.4, 3.2, 1.3, 0.2, 1, 0 };
      trainData[35] = new double[] { 5.0, 3.5, 1.6, 0.6, 1, 0 };
      trainData[36] = new double[] { 5.1, 3.8, 1.9, 0.4, 1, 0 };
      trainData[37] = new double[] { 4.6, 3.2, 1.4, 0.2, 1, 0 };
      trainData[38] = new double[] { 5.3, 3.7, 1.5, 0.2, 1, 0 };
      trainData[39] = new double[] { 5.0, 3.3, 1.4, 0.2, 1, 0 };

      trainData[40] = new double[] { 6.9, 3.1, 4.9, 1.5, 0, 1 }; // versicolor
      trainData[41] = new double[] { 5.5, 2.3, 4.0, 1.3, 0, 1 };
      trainData[42] = new double[] { 6.5, 2.8, 4.6, 1.5, 0, 1 };
      trainData[43] = new double[] { 5.7, 2.8, 4.5, 1.3, 0, 1 };
      trainData[44] = new double[] { 6.3, 3.3, 4.7, 1.6, 0, 1 };
      trainData[45] = new double[] { 4.9, 2.4, 3.3, 1.0, 0, 1 };
      trainData[46] = new double[] { 6.6, 2.9, 4.6, 1.3, 0, 1 };
      trainData[47] = new double[] { 5.2, 2.7, 3.9, 1.4, 0, 1 };
      trainData[48] = new double[] { 5.0, 2.0, 3.5, 1.0, 0, 1 };
      trainData[49] = new double[] { 5.9, 3.0, 4.2, 1.5, 0, 1 };

      trainData[50] = new double[] { 6.0, 2.2, 4.0, 1.0, 0, 1 };
      trainData[51] = new double[] { 6.1, 2.9, 4.7, 1.4, 0, 1 };
      trainData[52] = new double[] { 5.6, 2.9, 3.6, 1.3, 0, 1 };
      trainData[53] = new double[] { 6.7, 3.1, 4.4, 1.4, 0, 1 };
      trainData[54] = new double[] { 5.6, 3.0, 4.5, 1.5, 0, 1 };
      trainData[55] = new double[] { 5.6, 2.5, 3.9, 1.1, 0, 1 };
      trainData[56] = new double[] { 5.9, 3.2, 4.8, 1.8, 0, 1 };
      trainData[57] = new double[] { 6.1, 2.8, 4.0, 1.3, 0, 1 };
      trainData[58] = new double[] { 6.3, 2.5, 4.9, 1.5, 0, 1 };
      trainData[59] = new double[] { 6.1, 2.8, 4.7, 1.2, 0, 1 };

      trainData[60] = new double[] { 6.4, 2.9, 4.3, 1.3, 0, 1 };
      trainData[61] = new double[] { 6.6, 3.0, 4.4, 1.4, 0, 1 };
      trainData[62] = new double[] { 6.8, 2.8, 4.8, 1.4, 0, 1 };
      trainData[63] = new double[] { 6.7, 3.0, 5.0, 1.7, 0, 1 };
      trainData[64] = new double[] { 5.5, 2.4, 3.8, 1.1, 0, 1 };
      trainData[65] = new double[] { 5.5, 2.4, 3.7, 1.0, 0, 1 };
      trainData[66] = new double[] { 5.8, 2.7, 3.9, 1.2, 0, 1 };
      trainData[67] = new double[] { 6.0, 3.4, 4.5, 1.6, 0, 1 };
      trainData[68] = new double[] { 6.7, 3.1, 4.7, 1.5, 0, 1 };
      trainData[69] = new double[] { 6.3, 2.3, 4.4, 1.3, 0, 1 };

      trainData[70] = new double[] { 5.6, 3.0, 4.1, 1.3, 0, 1 };
      trainData[71] = new double[] { 5.5, 2.5, 4.0, 1.3, 0, 1 };
      trainData[72] = new double[] { 5.5, 2.6, 4.4, 1.2, 0, 1 };
      trainData[73] = new double[] { 6.1, 3.0, 4.6, 1.4, 0, 1 };
      trainData[74] = new double[] { 5.8, 2.6, 4.0, 1.2, 0, 1 };
      trainData[75] = new double[] { 5.0, 2.3, 3.3, 1.0, 0, 1 };
      trainData[76] = new double[] { 5.6, 2.7, 4.2, 1.3, 0, 1 };
      trainData[77] = new double[] { 5.7, 3.0, 4.2, 1.2, 0, 1 };
      trainData[78] = new double[] { 5.7, 2.9, 4.2, 1.3, 0, 1 };
      trainData[79] = new double[] { 5.7, 2.8, 4.1, 1.3, 0, 1 };

      double[][] testData = new double[20][];
      testData[0] = new double[] { 5.1, 3.5, 1.4, 0.2, 1, 0 };
      testData[1] = new double[] { 4.9, 3.0, 1.4, 0.2, 1, 0 };
      testData[2] = new double[] { 5.8, 4.0, 1.2, 0.2, 1, 0 };
      testData[3] = new double[] { 5.7, 4.4, 1.5, 0.4, 1, 0 };
      testData[4] = new double[] { 4.8, 3.0, 1.4, 0.3, 1, 0 };
      testData[5] = new double[] { 5.1, 3.8, 1.6, 0.2, 1, 0 };
      testData[6] = new double[] { 5.2, 3.4, 1.4, 0.2, 1, 0 };
      testData[7] = new double[] { 4.7, 3.2, 1.6, 0.2, 1, 0 };
      testData[8] = new double[] { 4.6, 3.6, 1.0, 0.2, 1, 0 };
      testData[9] = new double[] { 5.1, 3.3, 1.7, 0.5, 1, 0 };

      testData[10] = new double[] { 7.0, 3.2, 4.7, 1.4, 0, 1 };
      testData[11] = new double[] { 6.4, 3.2, 4.5, 1.5, 0, 1 };
      testData[12] = new double[] { 5.8, 2.7, 4.1, 1.0, 0, 1 };
      testData[13] = new double[] { 6.2, 2.2, 4.5, 1.5, 0, 1 };
      testData[14] = new double[] { 6.0, 2.9, 4.5, 1.5, 0, 1 };
      testData[15] = new double[] { 5.7, 2.6, 3.5, 1.0, 0, 1 };
      testData[16] = new double[] { 6.2, 2.9, 4.3, 1.3, 0, 1 };
      testData[17] = new double[] { 5.1, 2.5, 3.0, 1.1, 0, 1 };
      testData[18] = new double[] { 6.0, 2.7, 5.1, 1.6, 0, 1 };
      testData[19] = new double[] { 5.4, 3.0, 4.5, 1.5, 0, 1 };

      Console.WriteLine("\nThe encoded training data is:\n");
      ShowMatrix(trainData, 4, 1, true);

      Console.WriteLine("\nThe encoded test data is:\n");
      ShowMatrix(testData, 4, 1, true);

      int numInput = 4; // number features
      int numHidden = 5;
      int numOutput = 2; // binary classification

      Console.WriteLine("Creating a 4-5-2 neural network");
      Console.WriteLine("Using tanh hidden layer activation");
      Console.WriteLine("Using binary softmax output layer activation");
      NeuralNetwork nn = new NeuralNetwork(numInput, numHidden, numOutput);

      int maxEpochs = 100;
      double learnRate = 0.05;
      double momentum = 0.01;
      Console.WriteLine("\nSetting maxEpochs = " + maxEpochs);
      Console.WriteLine("Setting learnRate = " + learnRate.ToString("F2"));
      Console.WriteLine("Setting momentum  = " + momentum.ToString("F2"));

      Console.WriteLine("\nStarting training using stocastic back-propagation)");
      double[] weights = nn.Train(trainData, maxEpochs, learnRate, momentum);
      Console.WriteLine("Finished training");
      Console.WriteLine("\nFinal neural network model weights:\n");
      ShowVector(weights, 4, 8, true);

      double trainAcc = nn.Accuracy(trainData);
      Console.WriteLine("\nFinal accuracy on training data = " + trainAcc.ToString("F4"));

      double testAcc = nn.Accuracy(testData);
      Console.WriteLine("Final accuracy on test data     = " + testAcc.ToString("F4"));

      double[] unknown = new double[] { 5.3, 3.0, 2.0, 1.0 };
      Console.WriteLine("\nPredicting species when features = ");
      ShowVector(unknown, 1, unknown.Length, true);
      string pred = nn.Predict(unknown);
      Console.WriteLine("\nPredicted species = " + pred);

      Console.WriteLine("\nEnd demo\n");
      Console.ReadLine();
    } // Main

    public static void ShowMatrix(double[][] matrix, int numRows,
      int decimals, bool indices)
    {
      int len = matrix.Length.ToString().Length;
      for (int i = 0; i < numRows; ++i)
      {
        if (indices == true)
          Console.Write("[" + i.ToString().PadLeft(len) + "]  ");
        for (int j = 0; j < matrix[i].Length; ++j)
        {
          double v = matrix[i][j];
          if (v >= 0.0)
            Console.Write(" "); // '+'
          Console.Write(v.ToString("F" + decimals) + "  ");
        }
        Console.WriteLine("");
      }

      if (numRows < matrix.Length)
      {
        Console.WriteLine(". . .");
        int lastRow = matrix.Length - 1;
        if (indices == true)
          Console.Write("[" + lastRow.ToString().PadLeft(len) + "]  ");
        for (int j = 0; j < matrix[lastRow].Length; ++j)
        {
          double v = matrix[lastRow][j];
          if (v >= 0.0)
            Console.Write(" "); // '+'
          Console.Write(v.ToString("F" + decimals) + "  ");
        }
      }
      Console.WriteLine("\n");
    }

    public static void ShowVector(double[] vector, int decimals,
      int lineLen, bool newLine)
    {
      for (int i = 0; i < vector.Length; ++i)
      {
        if (i > 0 && i % lineLen == 0) Console.WriteLine("");
        if (vector[i] >= 0) Console.Write(" ");
        Console.Write(vector[i].ToString("F" + decimals) + " ");
      }
      if (newLine == true)
        Console.WriteLine("");
    }

  } // Program

  // ----------------

  public class NeuralNetwork
  {
    private int numInput; // number input nodes
    private int numHidden;
    private int numOutput;

    private double[] inputs;
    private double[][] ihWeights; // input-hidden
    private double[] hBiases;
    private double[] hOutputs;

    private double[][] hoWeights; // hidden-output
    private double[] oBiases;
    private double[] outputs;

    private Random rnd;

    public NeuralNetwork(int numInput, int numHidden, int numOutput)
    {
      this.numInput = numInput;
      this.numHidden = numHidden;
      this.numOutput = numOutput;

      this.inputs = new double[numInput];

      this.ihWeights = MakeMatrix(numInput, numHidden, 0.0);
      this.hBiases = new double[numHidden];
      this.hOutputs = new double[numHidden];

      this.hoWeights = MakeMatrix(numHidden, numOutput, 0.0);
      this.oBiases = new double[numOutput];
      this.outputs = new double[numOutput];

      this.rnd = new Random(0);
      this.InitializeWeights(); // all weights and biases
    } // ctor

    private static double[][] MakeMatrix(int rows,
      int cols, double v) // helper for ctor, Train
    {
      double[][] result = new double[rows][];
      for (int r = 0; r < result.Length; ++r)
        result[r] = new double[cols];
      for (int i = 0; i < rows; ++i)
        for (int j = 0; j < cols; ++j)
          result[i][j] = v;
      return result;
    }

    private void InitializeWeights() // helper for ctor
    {
      // initialize weights and biases to random values between 0.0001 and 0.001
      int numWeights = (numInput * numHidden) +
        (numHidden * numOutput) + numHidden + numOutput;
      double[] initialWeights = new double[numWeights];
      for (int i = 0; i < initialWeights.Length; ++i)
        initialWeights[i] = (0.001 - 0.0001) * rnd.NextDouble() + 0.0001;
      this.SetWeights(initialWeights);
    }

    public void SetWeights(double[] weights)
    {
      // copy serialized weights and biases in weights[] array
      // to i-h weights, i-h biases, h-o weights, h-o biases
      int numWeights = (numInput * numHidden) +
        (numHidden * numOutput) + numHidden + numOutput;
      if (weights.Length != numWeights)
        throw new Exception("Bad weights array in SetWeights");

      int k = 0; // points into weights param

      for (int i = 0; i < numInput; ++i)
        for (int j = 0; j < numHidden; ++j)
          ihWeights[i][j] = weights[k++];
      for (int i = 0; i < numHidden; ++i)
        hBiases[i] = weights[k++];
      for (int i = 0; i < numHidden; ++i)
        for (int j = 0; j < numOutput; ++j)
          hoWeights[i][j] = weights[k++];
      for (int i = 0; i < numOutput; ++i)
        oBiases[i] = weights[k++];
    }

    public double[] GetWeights()
    {
      int numWeights = (numInput * numHidden) +
        (numHidden * numOutput) + numHidden + numOutput;
      double[] result = new double[numWeights];
      int k = 0;
      for (int i = 0; i < ihWeights.Length; ++i)
        for (int j = 0; j < ihWeights[0].Length; ++j)
          result[k++] = ihWeights[i][j];
      for (int i = 0; i < hBiases.Length; ++i)
        result[k++] = hBiases[i];
      for (int i = 0; i < hoWeights.Length; ++i)
        for (int j = 0; j < hoWeights[0].Length; ++j)
          result[k++] = hoWeights[i][j];
      for (int i = 0; i < oBiases.Length; ++i)
        result[k++] = oBiases[i];
      return result;
    }

    public double[] ComputeOutputs(double[] xValues)
    {
      double[] hSums = new double[numHidden]; // hidden nodes sums scratch array
      double[] oSums = new double[numOutput]; // output nodes sums

      for (int i = 0; i < xValues.Length; ++i) // copy x-values to inputs
        this.inputs[i] = xValues[i];
      // note: no need to copy x-values unless you implement a ToString.
      // more efficient is to simply use the xValues[] directly.


      for (int j = 0; j < numHidden; ++j)  // compute i-h sum of weights * inputs
        for (int i = 0; i < numInput; ++i)
          hSums[j] += this.inputs[i] * this.ihWeights[i][j]; // note +=

      for (int i = 0; i < numHidden; ++i)  // add biases to input-to-hidden sums
        hSums[i] += this.hBiases[i];

      for (int i = 0; i < numHidden; ++i)   // apply activation
        this.hOutputs[i] = HyperTan(hSums[i]); // hard-coded

      for (int j = 0; j < numOutput; ++j)   // compute h-o sum of weights * hOutputs
        for (int i = 0; i < numHidden; ++i)
          oSums[j] += hOutputs[i] * hoWeights[i][j];

      for (int i = 0; i < numOutput; ++i)  // add biases to input-to-hidden sums
        oSums[i] += oBiases[i];

      double[] softOut = Softmax(oSums); // all outputs at once for efficiency
      Array.Copy(softOut, outputs, softOut.Length);
     
      double[] retResult = new double[numOutput]; // could define a GetOutputs 
      Array.Copy(this.outputs, retResult, retResult.Length);
      return retResult;
    }

    private static double HyperTan(double x)
    {
      if (x < -20.0) return -1.0; // approximation is correct to 30 decimals
      else if (x > 20.0) return 1.0;
      else return Math.Tanh(x);
    }

    private static double[] Softmax(double[] oSums)
    {
      // does all output nodes at once so scale
      // doesn't have to be re-computed each time

      // if (oSums.Length < 2) throw . . . 

      double[] result = new double[oSums.Length];
      
      double sum = 0.0;
      for (int i = 0; i < oSums.Length; ++i)
        sum += Math.Exp(oSums[i]);
      
      for (int i = 0; i < oSums.Length; ++i)
        result[i] = Math.Exp(oSums[i]) / sum;

      return result; // now scaled so that xi sum to 1.0
    }

    public double[] Train(double[][] trainData,
       int maxEpochs, double learnRate, double momentum)
    {
      // train using back-prop
      // back-prop specific arrays
      double[][] hoGrads = MakeMatrix(numHidden, numOutput, 0.0); // hidden-to-output weights gradients
      double[] obGrads = new double[numOutput];                   // output biases gradients

      double[][] ihGrads = MakeMatrix(numInput, numHidden, 0.0);  // input-to-hidden weights gradients
      double[] hbGrads = new double[numHidden];                   // hidden biases gradients

      double[] oSignals = new double[numOutput];                  // output signals - gradients w/o associated input terms
      double[] hSignals = new double[numHidden];                  // hidden node signals

      // back-prop momentum specific arrays 
      double[][] ihPrevWeightsDelta = MakeMatrix(numInput, numHidden, 0.0);
      double[] hPrevBiasesDelta = new double[numHidden];
      double[][] hoPrevWeightsDelta = MakeMatrix(numHidden, numOutput, 0.0);
      double[] oPrevBiasesDelta = new double[numOutput];

      // train a back-prop style NN classifier using learning rate and momentum
      int epoch = 0;
      double[] xValues = new double[numInput]; // inputs
      double[] tValues = new double[numOutput]; // target values

      int[] sequence = new int[trainData.Length];
      for (int i = 0; i < sequence.Length; ++i)
        sequence[i] = i;

      int errInterval = maxEpochs / 10; // interval to check validation data
      while (epoch < maxEpochs)
      {
        ++epoch;

        if (epoch % errInterval == 0 && epoch < maxEpochs)
        {
          double trainErr = Error(trainData);
          Console.WriteLine("epoch = " + epoch + "  training error = " +
            trainErr.ToString("F4"));
          //Console.ReadLine();
        }

        Shuffle(sequence); // visit each training data in random order
        for (int ii = 0; ii < trainData.Length; ++ii)
        {
          int idx = sequence[ii];
          Array.Copy(trainData[idx], xValues, numInput);
          Array.Copy(trainData[idx], numInput, tValues, 0, numOutput);
          ComputeOutputs(xValues); // copy xValues in, compute outputs 

          // indices: i = inputs, j = hiddens, k = outputs

          // 1. compute output nodes signals (assumes softmax)
          for (int k = 0; k < numOutput; ++k)
            oSignals[k] = (tValues[k] - outputs[k]) * (1 - outputs[k]) * outputs[k];

          // 2. compute hidden-to-output weights gradients using output signals
          for (int j = 0; j < numHidden; ++j)
          {
            for (int k = 0; k < numOutput; ++k)
            {
              hoGrads[j][k] = oSignals[k] * hOutputs[j];
            }
          }

          // 2b. compute output biases gradients using output signals
          for (int k = 0; k < numOutput; ++k)
            obGrads[k] = oSignals[k] * 1.0; // dummy assoc. input value

          // 3. compute hidden nodes signals
          for (int j = 0; j < numHidden; ++j)
          {
            double sum = 0.0; // need sums of output signals times hidden-to-output weights
            for (int k = 0; k < numOutput; ++k)
            {
              sum += oSignals[k] * hoWeights[j][k];
            }
            hSignals[j] = (1 + hOutputs[j]) * (1 - hOutputs[j]) * sum;  // assumes tanh
          }

          // 4. compute input-hidden weights gradients
          for (int i = 0; i < numInput; ++i)
            for (int j = 0; j < numHidden; ++j)
              ihGrads[i][j] = hSignals[j] * inputs[i];

          // 4b. compute hidden node biases gradienys
          for (int j = 0; j < numHidden; ++j)
            hbGrads[j] = hSignals[j] * 1.0; // dummy 1.0 input

          // == update weights and biases

          // update input-to-hidden weights
          for (int i = 0; i < numInput; ++i)
          {
            for (int j = 0; j < numHidden; ++j)
            {
              double delta = ihGrads[i][j] * learnRate;
              ihWeights[i][j] += delta;
              ihWeights[i][j] += ihPrevWeightsDelta[i][j] * momentum;
              ihPrevWeightsDelta[i][j] = delta; // save for next time
            }
          }

          // update hidden biases
          for (int j = 0; j < numHidden; ++j)
          {
            double delta = hbGrads[j] * learnRate;
            hBiases[j] += delta;
            hBiases[j] += hPrevBiasesDelta[j] * momentum;
            hPrevBiasesDelta[j] = delta;
          }

          // update hidden-to-output weights
          for (int j = 0; j < numHidden; ++j)
          {
            for (int k = 0; k < numOutput; ++k)
            {
              double delta = hoGrads[j][k] * learnRate;
              hoWeights[j][k] += delta;
              hoWeights[j][k] += hoPrevWeightsDelta[j][k] * momentum;
              hoPrevWeightsDelta[j][k] = delta;
            }
          }

          // update output node biases
          for (int k = 0; k < numOutput; ++k)
          {
            double delta = obGrads[k] * learnRate;
            oBiases[k] += delta;
            oBiases[k] += oPrevBiasesDelta[k] * momentum;
            oPrevBiasesDelta[k] = delta;
          }

        } // each training item

      } // while
      double[] bestWts = GetWeights();
      return bestWts;
    } // Train


    private void Shuffle(int[] sequence) // an instance method
    {
      for (int i = 0; i < sequence.Length; ++i)
      {
        int r = this.rnd.Next(i, sequence.Length);
        int tmp = sequence[r];
        sequence[r] = sequence[i];
        sequence[i] = tmp;
      }
    } // Shuffle

    private double Error(double[][] data)
    {
      // average squared error per training item
      double sumSquaredError = 0.0;
      double[] xValues = new double[numInput]; // first numInput values in trainData
      double[] tValues = new double[numOutput]; // last numOutput values

      // walk thru each training case. looks like (6.9 3.2 5.7 2.3) (0 0 1)
      for (int i = 0; i < data.Length; ++i)
      {
        Array.Copy(data[i], xValues, numInput);
        Array.Copy(data[i], numInput, tValues, 0, numOutput); // get target values
        double[] yValues = this.ComputeOutputs(xValues); // outputs using current weights
        for (int j = 0; j < numOutput; ++j)
        {
          double err = tValues[j] - yValues[j];
          sumSquaredError += err * err;
        }
      }
      return sumSquaredError / data.Length;
    } // MeanSquaredError

    public double Accuracy(double[][] data)
    {
      // percentage correct using winner-takes all
      int numCorrect = 0;
      int numWrong = 0;
      double[] xValues = new double[numInput]; // inputs
      double[] tValues = new double[numOutput]; // targets
      double[] yValues; // computed Y

      for (int i = 0; i < data.Length; ++i)
      {
        Array.Copy(data[i], xValues, numInput); // get x-values
        Array.Copy(data[i], numInput, tValues, 0, numOutput); // get t-values
        yValues = this.ComputeOutputs(xValues);
        int maxIndex = MaxIndex(yValues); // which cell in yValues has largest value?
        int tMaxIndex = MaxIndex(tValues);

        if (maxIndex == tMaxIndex)
          ++numCorrect;
        else
          ++numWrong;

        //if (tValues[maxIndex] == 1.0) // bad idea.
        //  ++numCorrect;
        //else
        //  ++numWrong;
      }
      return (numCorrect * 1.0) / (numCorrect + numWrong);
    }

    private static int MaxIndex(double[] vector) // helper for Accuracy()
    {
      // index of largest value
      int bigIndex = 0;
      double biggestVal = vector[0];
      for (int i = 0; i < vector.Length; ++i)
      {
        if (vector[i] > biggestVal)
        {
          biggestVal = vector[i];
          bigIndex = i;
        }
      }
      return bigIndex;
    }

    public string Predict(double[] xValues)
    {
      // specific to binary Iris data
      // (1, 0) = setosa, (0, 1) = versicolor
      double[] computedY = this.ComputeOutputs(xValues);
      int maxIndex = MaxIndex(computedY); // which cell in yValues has largest value?
      if (maxIndex == 0)
        return "setosa";
      else
        return "versicolor";
    }


  } // NeuralNetwork
  // ----------------

} // ns
