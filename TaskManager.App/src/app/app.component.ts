import { Component } from '@angular/core';
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
export class AppComponent {

    tasks: KanbanTask[] = [];

    constructor(public service: KanbanTaskService,
        private toastr: ToastrService) { }

    onSubmit(form: NgForm) {
        if (!form.value.title || !form.value.conclusionDate)
            this.toastr.error("É necessário um título");

        if (!form.value.conclusionDate)
            this.toastr.error("É necessário uma data de conclusão");

        let kanbanTask = new KanbanTask(form.value.title, KanbanTaskStatus.Doing, form.value.conclusionDate);
        this.onInsert(kanbanTask, form);
    }

    onInsert(kanbanTask: KanbanTask, formData: NgForm) {
        this.service.postKanbanTask(kanbanTask).subscribe(
            res => {
                formData.form.reset();
                this.service.refreshList();
                this.toastr.success("Tarefa Criada", "Kanban Task");
            },
            err => { console.log(err); }
        );
    }

    onComplete(kanbanTask: KanbanTask) {
        kanbanTask.status = KanbanTaskStatus.Done;
        this.updateTask(kanbanTask);
    }

    onDelete(taksId: number) {
        this.service.deleteKanbanTask(taksId).subscribe(
            res => {
                this.service.refreshList();
                this.toastr.success("Tarefa Excluída", "Kanban Task");
            },
            err => { console.log(err); }
        );
    }

    updateTask(kanbanTask: KanbanTask) {
        this.service.putKanbanTask(kanbanTask).subscribe(
            res => {
                this.service.refreshList();
                this.toastr.success("Tarefa Atualizada", "Kanban Task");
            },
            err => { console.log(err); }
        );
    }
}