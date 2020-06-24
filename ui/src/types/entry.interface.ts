export interface IEntry
{
    id:string,
    name: string,
    phoneNumber: string
}

export class EntryModel implements IEntry
{
    id: string;
    name: string;
    phoneNumber: string;
    //ui properties
    selected:boolean;
    deleteClass:string;
}