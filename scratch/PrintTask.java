import com.langla.data.DataCenter;
import com.langla.data.Task;
import com.langla.data.Step;

public class PrintTask {
    public static void main(String[] args) {
        try {
            DataCenter.gI().readArrDataGame(false);
            if (DataCenter.gI().Task != null) {
                for (int t = 0; t < Math.min(5, DataCenter.gI().Task.length); t++) {
                    Task task = DataCenter.gI().Task[t];
                    System.out.println("=== Task " + task.id + ": " + task.name + " (NPC: " + task.idNpc + ") ===");
                    for (int s = 0; s < task.vStep.size(); s++) {
                        Step step = (Step) task.vStep.get(s);
                        System.out.println("  Step " + s + ": id=" + step.id + ", name=[" + step.name + "], STR=[" + step.STR + "], idNpc=" + step.idNpc + ", idItem=" + step.idItem);
                    }
                }
            } else {
                System.out.println("Task array is null!");
            }
        } catch (Exception e) {
            e.printStackTrace();
        }
    }
}

