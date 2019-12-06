namespace Panner.Interfaces
{
    public interface IParticleGenerator<T>
    {
        bool TryGenerate(IPContext context, string input, out T particle);
    }
}
