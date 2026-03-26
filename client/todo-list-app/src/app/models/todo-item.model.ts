export interface TodoItem {
    id: number;
    title: string;
    description?: string;
    isCompleted: boolean;
    maxCompletionDate: string;
    createdAt: string;
}

export interface CreateTodoItem {
    title: string;
    description?: string;
    maxCompletionDate: string;
}

export interface UpdateTodoItem {
    title: string;
    description?: string;
    isCompleted: boolean;
    maxCompletionDate: string;
}