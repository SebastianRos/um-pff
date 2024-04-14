using System.Collections.Generic;

public interface Observable {
    public void register(Observer observer);
    public void unregister(Observer observer);
}