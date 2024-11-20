namespace Survay_Basket.API.Services;

public interface IPollService
{
    Poll Add(Poll poll);
    bool Update(int id, Poll poll);
    bool Delete(int id);
    ICollection<Poll> GetAll();
    Poll? GetById(int id);
}
