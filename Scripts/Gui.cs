using System.Threading.Tasks;
using Godot;
public partial class Gui : Control
{
	[Export] public GameManager gameManager;
	[Export] public Panel GameOverLabel;
	[Export] public Label ExperienceLabel;
	[Export] public Label HealthLabel;
	[Export] public Label StatusLabel;
	[Export] public Label MissionLabel;
	[Export ] public Label LevelUpQtyLabel;
	[Export] public Panel LevelUpPanel;
	[Export] public Label WaveLabel;
	[Export] public Panel WinPanel;


	public override void _Ready()
	{
		GameOverLabel.Visible = false;
		LevelUpPanel.Visible = false;
		gameManager.OnPlayerDied += ShowGameOver;
		gameManager.MissionUpdateEvent += UpdateMission;
		gameManager.HealthUpdateEvent += UpdateHealth;
		gameManager.ExperienceUpdateEvent += UpdateExperience;
		gameManager.StartWaveNoticeEvent += async (wave) => await ShowTemporaryMessage("Wave " + wave, 3.0f);
		gameManager.LevelUp += LevelUp;
		gameManager.GuiStatusUpdateEvent += UpdateStats;
		gameManager.FinishLastWave += FinishLastWave;
		this.ProcessMode = ProcessModeEnum.Always;

	}
	private void UpdateHealth(string healthLabel)
	{
		//GD.Print("Updating Health Label: " + healthLabel);
		HealthLabel.Text = healthLabel;
	}
	private void UpdateExperience(string healthLabel)
	{
		//GD.Print("Updating Experience Label: " + healthLabel);
		ExperienceLabel.Text = healthLabel;
	}

	private void UpdateStats(StatsComponent stats)
	{
		//GD.Print("Updating Stats");
		StatusLabel.Text = "ATK: " + stats.CurrentAttack.ToString("F2") + "\n SPD: " + stats.CurrentSpeed.ToString("F2") + "\n DEX: " + stats.CurrentDexterity.ToString("F2");
	}

    private void ShowGameOver()
    {
		//GD.Print("Displaying Game Over Screen");
        GameOverLabel.Visible = true;
    }
	private void UpdateMission(string missionText)
	{
		//GD.Print("Updating Mission Label: " + missionText);
		MissionLabel.Text = missionText;
	}

	public void OnTryAgainPressed()
	{
		//GD.Print("Reloading scene: ");
		var currentScenePath = GetTree().CurrentScene.SceneFilePath;
		//GD.Print("Reloading scene: " + currentScenePath);
		GetTree().ChangeSceneToFile(currentScenePath);
	}
	int levelUpQty = 0;
	private void LevelUp()
    {
		//GD.Print("Level Up available, level qty : " + (levelUpQty + 1));
        levelUpQty++;
        if (levelUpQty == 1 && !LevelUpPanel.Visible)
		{
			UpdateLevelUpQtyLabel();
			LevelUpPanel.Visible = true;
		}
		UpdateLevelUpQtyLabel();
    }
	private void UpdateLevelUpQtyLabel()
	{
		LevelUpQtyLabel.Text = "Level Up Points: \n" + levelUpQty;
	}
	public void LevelUpChoice(int choice)
	{

		levelUpQty--;
		UpdateLevelUpQtyLabel();

		gameManager.PlayerStatsUpdateEvent?.Invoke((UpdateStatus)choice);
		LevelUpPanel.Visible = levelUpQty != 0;
		//GD.Print("Level Up Choice made: " + choice);	
	}

	public async Task ShowTemporaryMessage(string message, float durationSeconds)
	{
		WaveLabel.Visible = true;
		//GD.Print("Showing temporary message: " + message);
		WaveLabel.Text = message;
		await ToSignal(GetTree().CreateTimer(durationSeconds), "timeout");
		WaveLabel.Visible = false;
	}

	public void FinishLastWave()
    {
		WinPanel.Visible = true;
        GetTree().Paused = true;
    }

	public void OnContinuePressed()
	{
		GetTree().Paused = false;
		WinPanel.Visible = false;
	}

}
