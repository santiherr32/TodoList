import { Routes } from '@angular/router';

export const routes: Routes = [
    { path: '', redirectTo: 'todos', pathMatch: 'full' },
    {
        path: 'todos',
        loadComponent: () =>
            import('./features/todos/todo-list/todo-list')
                .then(m => m.TodoList)
    },
    {
        path: 'todos/new',
        loadComponent: () =>
            import('./features/todos/todo-form/todo-form')
                .then(m => m.TodoForm)
    },
    {
        path: 'todos/edit/:id',
        loadComponent: () =>
            import('./features/todos/todo-form/todo-form')
                .then(m => m.TodoForm)
    }
];