import { Deserializable } from './deserializable.model';

export class ToDoItem implements Deserializable{
    description: string;
    id: string;
    toDoListId: string;
    position: number;
    completed: boolean;

    deserialize(input: any) {
        Object.assign(this, input);
        return this;
    }
}
