using Xunit;
using MusicMixerApp;

public class MixerTests
{
    [Fact]
    public void MixShouldReturnCombinedMelody()
    {
        // TODO: проверить, что метод Mix возвращает не null
        var mixer = new Mixer();

        var result = mixer.Mix("melody1.wav", "melody2.wav");

        Assert.NotNull(result);
    }

    [Fact]
    public void Mix_WithInvalidPaths_ShouldThrowException()
    {
        // TODO: обработать несуществующие пути в методе Mix
        var mixer = new Mixer();

        Assert.Throws<FileNotFoundException>(() => mixer.Mix("nonexistent1.wav", "nonexistent2.wav"));
    }
}