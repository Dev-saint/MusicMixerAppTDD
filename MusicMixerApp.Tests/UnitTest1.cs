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

    [Fact]
    public void Mix_WithInvalidPaths_ShouldThrowException()
    {
        // TODO: ���������� �������������� ���� � ������ Mix
        var mixer = new Mixer();

        Assert.Throws<FileNotFoundException>(() => mixer.Mix("nonexistent1.wav", "nonexistent2.wav"));
    }
}