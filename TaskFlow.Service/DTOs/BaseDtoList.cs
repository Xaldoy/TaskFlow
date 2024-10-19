namespace TaskFlow.Service.DTOs
{
    public class BaseDtoList : BaseDto
    {
        public List<BaseDto> Data { get; set; } = [];

        public BaseDtoList() { }

        public BaseDtoList(IEnumerable<BaseDto> items)
        {
            Data = new List<BaseDto>(items);
        }
    }
}
