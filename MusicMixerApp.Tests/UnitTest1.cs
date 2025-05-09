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
}