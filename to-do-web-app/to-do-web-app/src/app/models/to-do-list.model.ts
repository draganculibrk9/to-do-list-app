import { ToDoItem } from './to-do-item.model';
import { Deserializable } from './deserializable.model';

export class ToDoList implements Deserializable{
    title: string;
    items: ToDoItem[] = [];
    id: string;
    reminderDate: Date;
    position: number;
    reminded: boolean;

    deserialize(input: any) {
        Object.assign(this, input);
        
        for(let item of input.items) {
            this.items.push(new ToDoItem().deserialize(item));
        }
        return this;
    }
}
