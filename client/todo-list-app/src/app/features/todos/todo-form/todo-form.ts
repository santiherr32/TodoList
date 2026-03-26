import { Component, inject, OnInit, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { TodoService } from '../../../core/services/todo';

@Component({
  selector: 'app-todo-form',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './todo-form.html',
})
export class TodoForm implements OnInit {
  private readonly todoService = inject(TodoService);
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);
  private readonly fb = inject(FormBuilder);

  isEditMode = signal(false);
  submitting = signal(false);
  serverErrors = signal<string[]>([]);
  private editId: number | null = null;

  today = new Date().toISOString().split('T')[0];

  form = this.fb.group({
    title: ['', [Validators.required, Validators.maxLength(40)]],
    description: ['', [Validators.maxLength(200)]],
    maxCompletionDate: ['', Validators.required],
    isCompleted: [false]
  });

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode.set(true);
      this.editId = +id;
      this.todoService.getById(this.editId).subscribe({
        next: (todo) => {
          this.form.patchValue({
            title: todo.title,
            description: todo.description ?? '',
            maxCompletionDate: todo.maxCompletionDate.split('T')[0],
            isCompleted: todo.isCompleted
          });
        }
      });
    }
  }

  submit(): void {
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }
    this.submitting.set(true);
    this.serverErrors.set([]);

    const { title, description, maxCompletionDate, isCompleted } = this.form.value;

    if (this.isEditMode() && this.editId !== null) {
      this.todoService.update(this.editId, {
        title: title!,
        description: description || undefined,
        isCompleted: isCompleted ?? false,
        maxCompletionDate: maxCompletionDate!
      }).subscribe({
        next: () => this.router.navigate(['/todos']),
        error: (err) => this.handleError(err)
      });
    } else {
      this.todoService.create({
        title: title!,
        description: description || undefined,
        maxCompletionDate: maxCompletionDate!
      }).subscribe({
        next: () => this.router.navigate(['/todos']),
        error: (err) => this.handleError(err)
      });
    }
  }

  private handleError(err: any): void {
    this.submitting.set(false);
    if (err.status === 400 && Array.isArray(err.error)) {
      this.serverErrors.set(err.error);
    } else {
      this.serverErrors.set(['Ocurrió un error inesperado. Intenta de nuevo.']);
    }
  }

  isInvalid(field: string): boolean {
    const c = this.form.get(field);
    return !!(c?.invalid && c?.touched);
  }

  getError(field: string): string {
    const c = this.form.get(field);
    if (c?.hasError('required')) return 'Este campo es obligatorio.';
    if (c?.hasError('maxlength')) return `Máximo ${c.errors?.['maxlength'].requiredLength} caracteres.`;
    return '';
  }

  goBack(): void {
    this.router.navigate(['/todos']);
  }
}