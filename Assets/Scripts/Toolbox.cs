/**
 * Specific toolbox implementation to store global variables.
 */

public class Toolbox : Singleton<Toolbox> {

	protected Toolbox () {} // guarantee this will be always a singleton only - can't use the constructor!
	
	// Currently user score
	public long currentScore = 0;
	
	void Awake () {
		// Your initialization code here
	}
}
