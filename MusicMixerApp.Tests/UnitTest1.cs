using Xunit;

public class MixerTests
{
    [Fact]
    public void Mix_ShouldReturnCombinedMelody()
    {
        // TODO: ���������, ��� ����� Mix ���������� �� null
        var mixer = new Mixer();

        var result = mixer.Mix("melody1.wav", "melody2.wav");

        Assert.NotNull(result);
    }
}