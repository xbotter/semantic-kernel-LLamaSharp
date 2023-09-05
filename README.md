⚠️**The implementation of this project has been migrated to the [LlamaSharp](https://github.com/SciSharp/LLamaSharp) repository, and maintenance will be continued there in the future.**

---

# semantic-kernel-LLamaSharp

Using [LLamaSharp](https://github.com/SciSharp/LLamaSharp) to implement custom TextCompletion and EmbeddingGeneration of [Semantic Kernel](https://github.com/microsoft/semantic-kernel)

# Basic Usage

1. Clone this repository.
2. Download a llama model. If you don't know which model is available, please refer to [LLamaSharp Verified Models](https://github.com/SciSharp/LLamaSharp#installation).
3. Modify the path of model and prompt file in `samples/SK_LLamaSharpWebApi/appsettings.json`.
4. Change the backend type if you want to, by modifying `SK_LLamaSharpWebApi.csproj` and `Directory.Packages.props`.
5. Run the sample. It's recommended to run with release mode or x64 configuration.
6. Send the request to the web api to test.

