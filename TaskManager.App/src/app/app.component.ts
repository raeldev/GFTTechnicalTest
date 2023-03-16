import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { KanbanTask } from "src/app/models/KanbanTask.model";
import { KanbanTaskStatus } from './enums/KanbanTaskStatus.enum';
import { ToastrService } from 'ngx-toastr';
import { KanbanTaskService } from './services/kanban-task.service';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

    // delay for wait worker persists
    seconds: number = 4;
    tasks: KanbanTask[] = [];

    constructor(public service: KanbanTaskService,
        private toastr: ToastrService) { }

    ngOnInit(){
        this.RefreshList();
    }
    
    onSubmit(form: NgForm) {
        if (!form.value.description)
        {
            this.toastr.error("É necessário um título");
            return;
        }

        if (!form.value.conclusionDate)
        {
            this.toastr.error("É necessário uma data de conclusão");
            return;
        }

        let kanbanTask = new KanbanTask(form.value.description, KanbanTaskStatus.Doing, form.value.conclusionDate);

        this.onInsert(kanbanTask, form);
    }

    onInsert(kanbanTask: KanbanTask, formData: NgForm) {
        this.service.postKanbanTask(kanbanTask).subscribe(
            res => {
                formData.form.reset();
                
                // wait worker persist
                setTimeout(async () => {
                    await this.RefreshList();
                }, this.seconds * 1000);

                this.toastr.success("Criando Tarefa", "Kanban Task");
                this.tasks.push(kanbanTask);
            },
            err => { console.log(err); }
        );
    }

    onComplete(kanbanTask: KanbanTask) {
        kanbanTask.status = KanbanTaskStatus.Done;
        this.updateTask(kanbanTask);
    }

    onDelete(taskId: number) {
        this.service.deleteKanbanTask(taskId).subscribe(
            res => {
                
                // wait worker persist
                setTimeout(async () => {
                    await this.RefreshList();
                }, this.seconds * 1000);

                this.toastr.success("Excluindo Tarefa", "Kanban Task");
                this.tasks = this.tasks.filter(x => x.taskId !== taskId);
            },
            err => { console.log(err); }
        );
    }

    updateTask(kanbanTask: KanbanTask) {
        this.service.putKanbanTask(kanbanTask).subscribe(
            res => {
                
                // wait worker persist
                setTimeout(async () => {
                    await this.RefreshList();
                }, this.seconds * 1000);

                this.toastr.success("Atualizando Tarefa", "Kanban Task");
            },
            err => { console.log(err); }
        );
    }

    checkTaskDone(kanbanTask: KanbanTask) {
        return kanbanTask.status == KanbanTaskStatus.Done
    }

    formatDate(conclusionDate: Date) : string {
        let date = new Date(conclusionDate.toString());
        return `${date.getUTCDate().toString().padStart(2, '0')}/${date.getUTCMonth().toString().padStart(2, '0')}`;
    }

    async RefreshList() {
        await this.service.refreshList().then(l => this.tasks = l);
    }
}