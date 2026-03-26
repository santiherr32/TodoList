import { Component, inject, OnInit, signal } from '@angular/core';
import { DatePipe } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { TodoService } from '../../../core/services/todo';
import { TodoItem, CreateTodoItem, UpdateTodoItem } from '../../../models/todo-item.model';
import { LucideDynamicIcon } from '@lucide/angular';

@Component({
  selector: 'app-todo-list',
  standalone: true,
  imports: [DatePipe, ReactiveFormsModule, LucideDynamicIcon],
  templateUrl: './todo-list.html',
})
export class TodoList implements OnInit {
  private readonly todoService = inject(TodoService);
  private readonly router = inject(Router);
  private readonly fb = inject(FormBuilder);

  todos = signal<TodoItem[]>([]);
  loading = signal(true);
  dialogOpen = signal(false);
  submitting = signal(false);
  serverErrors = signal<string[]>([]);

  today = new Date().toISOString().split('T')[0];

  quickForm = this.fb.group({
    title: ['', [Validators.required, Validators.maxLength(40)]],
    description: ['', [Validators.maxLength(200)]],
    maxCompletionDate: ['', Validators.required],
  });

  ngOnInit(): void {
    this.loadTodos();
  }

  loadTodos(): void {
    this.loading.set(true);
    this.todoService.getAll().subscribe({
      next: (data) => { this.todos.set(data); this.loading.set(false); },
      error: () => this.loading.set(false)
    });
  }

  openDialog(): void {
    this.quickForm.reset();
    this.serverErrors.set([]);
    this.dialogOpen.set(true);
  }

  closeDialog(): void {
    this.dialogOpen.set(false);
  }

  submitQuick(): void {
    if (this.quickForm.invalid) {
      this.quickForm.markAllAsTouched();
      return;
    }
    this.submitting.set(true);
    this.serverErrors.set([]);

    const { title, description, maxCompletionDate } = this.quickForm.value;
    const dto: CreateTodoItem = {
      title: title!,
      description: description || undefined,
      maxCompletionDate: maxCompletionDate!
    };

    this.todoService.create(dto).subscribe({
      next: (created) => {
        this.todos.update(list => [...list, created]);
        this.submitting.set(false);
        this.closeDialog();
      },
      error: (err) => {
        this.submitting.set(false);
        if (err.status === 400 && Array.isArray(err.error)) {
          this.serverErrors.set(err.error);
        } else {
          this.serverErrors.set(['Error inesperado. Intenta de nuevo.']);
        }
      }
    });
  }

  toggleCompleted(todo: TodoItem): void {
    const dto: UpdateTodoItem = {
      title: todo.title,
      description: todo.description,
      isCompleted: !todo.isCompleted,
      maxCompletionDate: todo.maxCompletionDate
    };
    this.todoService.update(todo.id, dto).subscribe({
      next: (updated) => {
        this.todos.update(list => list.map(t => t.id === updated.id ? updated : t));
      }
    });
  }

  delete(id: number): void {
    if (!confirm('¿Seguro que deseas eliminar esta tarea?')) return;
    this.todoService.delete(id).subscribe({
      next: () => this.todos.update(list => list.filter(t => t.id !== id))
    });
  }

  goToEdit(id: number): void {
    this.router.navigate(['/todos/edit', id]);
  }

  getTitleError(): string {
    const c = this.quickForm.get('title');
    if (c?.hasError('required')) return 'El título es obligatorio.';
    if (c?.hasError('maxlength')) return 'Máximo 40 caracteres.';
    return '';
  }
}