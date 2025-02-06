using TSFiler.BusinessLogic.Services.DataProcessors;

namespace TSFiler.Tests.DataProcessorTests;

public class RegexDataProcessorTests
{
    private readonly RegexDataProcessor _processor;

    public RegexDataProcessorTests()
    {
        _processor = new RegexDataProcessor();
    }

    [Theory]
    [InlineData("2+3", "5")]
    [InlineData("4-2", "2")]
    [InlineData("6*7", "42")]
    [InlineData("8/2", "4")]
    [InlineData("2+4*9", "38")]
    [InlineData("(2+3)*4", "20")]
    [InlineData("10/(2+3)", "2")]
    [InlineData("2*(3+4)*2", "28")]
    [InlineData("{\"value\": 2+3, \"key\": 10+8}", "{\"value\": 5, \"key\": 18}")]
    [InlineData("{\"a\": 4*5, \"b\": {\"c\": 2+3, \"d\": 6/2}}", "{\"a\": 20, \"b\": {\"c\": 5, \"d\": 3}}")]
    [InlineData("{\"numbers\": [1+2, 3*4, 10-7], \"result\": 100/5}", "{\"numbers\": [3, 12, 3], \"result\": 20}")]
    [InlineData("{\"outer\": {\"inner\": {\"deep\": 8-3*2}}, \"key\": 10+5*2}", "{\"outer\": {\"inner\": {\"deep\": 2}}, \"key\": 20}")]
    [InlineData("{\"math\": [2*3+4, (5+3)*2, 6/2], \"operation\": 10+(6*7)}", "{\"math\": [10, 16, 3], \"operation\": 52}")]
    [InlineData("{\"nested\": {\"level1\": {\"level2\": {\"expression\": 10-3+2}}}, \"value\": 7*2}", "{\"nested\": {\"level1\": {\"level2\": {\"expression\": 9}}}, \"value\": 14}")]
    [InlineData("{\"arr\": [5+3, 10*(2+3)], \"obj\": {\"num\": 100/4, \"exp\": 9-3}}", "{\"arr\": [8, 50], \"obj\": {\"num\": 25, \"exp\": 6}}")]
    [InlineData("{\"calc\": {\"a\": 4*(6+2), \"b\": 3+5*(2+1), \"c\": 18/(3+3)}}", "{\"calc\": {\"a\": 32, \"b\": 18, \"c\": 3}}")]
    [InlineData("{\"result\": 7*(4+2)-3, \"expression\": {\"inner\": 5+(8*3)/4}}", "{\"result\": 39, \"expression\": {\"inner\": 11}}")]
    public void ProcessData_ValidInput_ReturnsCorrectResult(string input, string expectedOutput)
    {
        var result = _processor.ProcessData(input);

        Assert.Equal(expectedOutput, result);
    }

    [Fact]
    public void ProcessData_DivisionByZeroOccurs_ThrowsDivideByZeroException()
    {
        var input = "5/0";

        Assert.Throws<DivideByZeroException>(() => _processor.ProcessData(input));
    }

    [Theory]
    [InlineData("2+3-4", "1")]
    [InlineData("2*3+4", "10")]
    [InlineData("2*(3+5)-1", "15")]
    public void ProcessData_HandlesMixedOperatorsAndParentheses(string input, string expectedOutput)
    {
        var result = _processor.ProcessData(input);

        Assert.Equal(expectedOutput, result);
    }
}
