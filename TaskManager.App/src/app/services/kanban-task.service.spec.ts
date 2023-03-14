import { TestBed } from '@angular/core/testing';

import { KanbanTaskService } from './kanban-task.service';

describe('KanbanTaskService', () => {
  let service: KanbanTaskService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(KanbanTaskService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
