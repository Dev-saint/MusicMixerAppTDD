using Xunit;
using MusicMixerApp;

public class MixerTests
{
    [Fact]
    public void MixShouldReturnCombinedMelody()
    {
        // TODO: ���������, ��� ����� Mix ���������� �� null
        var mixer = new Mixer();

        var result = mixer.Mix("melody1.wav", "melody2.wav");

        Assert.NotNull(result);
    }
}