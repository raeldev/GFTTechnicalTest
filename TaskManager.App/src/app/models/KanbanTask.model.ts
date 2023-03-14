import { KanbanTaskStatus } from "../enums/KanbanTaskStatus.enum";

export class KanbanTask {
    taskId: number = 0;
    description: string = "";
    status: KanbanTaskStatus = KanbanTaskStatus.New;
    conclusionDate: Date = new Date();
    
    constructor(
        description: string,
        status: KanbanTaskStatus,
        conclusionDate: Date
    ) { }
}