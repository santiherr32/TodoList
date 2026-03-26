import { Component, inject, OnInit, signal } from '@angular/core';
import { Router } from '@angular/router';
import { TodoService } from '../../../core/services/todo';
import { TodoItem, UpdateTodoItem } from '../../../models/todo-item.model';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-todo-list',
  standalone: true,
  templateUrl: './todo-list.html',
  imports: [DatePipe],
})

export class TodoList implements OnInit {
  private readonly todoService = inject(TodoService);
  private readonly router = inject(Router);

  todos = signal<TodoItem[]>([]);
  loading = signal(true);

  ngOnInit(): void {
    this.loadTodos();
  }

  loadTodos(): void {
    this.loading.set(true);
    this.todoService.getAll().subscribe({
      next: (data) => {
        this.todos.set(data);
        this.loading.set(false);
      },
      error: () => this.loading.set(false),
    });
  }

  toggleCompleted(todo: TodoItem): void {
    const dto: UpdateTodoItem = {
      title: todo.title,
      description: todo.description,
      isCompleted: !todo.isCompleted,
      maxCompletionDate: todo.maxCompletionDate,
    };
    this.todoService.update(todo.id, dto).subscribe({
      next: (updated) => {
        this.todos.update((list) => list.map((t) => (t.id === updated.id ? updated : t)));
      },
    });
  }

  delete(id: number): void {
    if (!confirm('¿Seguro que deseas eliminar esta tarea?')) return;
    this.todoService.delete(id).subscribe({
      next: () => this.todos.update((list) => list.filter((t) => t.id !== id)),
    });
  }

  goToCreate(): void {
    this.router.navigate(['/todos/new']);
  }

  goToEdit(id: number): void {
    this.router.navigate(['/todos/edit', id]);
  }
}
