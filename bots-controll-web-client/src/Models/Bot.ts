import { User } from "./User"

export interface Bot {
    id: string
    name : string
    isActive: boolean
    owner: User
} 