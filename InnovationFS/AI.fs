module InnovationFS.AI

open InnovationFS.Cards
open InnovationFS.Player

type Action =
    | Achieve
    | Draw
    | Dogma
    | Play

type CardChoice = Card * Action

type Choices() =
    // Will enumerating the set change the AI strategy?
    // Not sure if order is important.
    // Could be a ResizeArray instead..
    let mutable all_choices = Set.empty<CardChoice>

    member x.GetAllChoices = all_choices

    member x.Add (card: Card) (action: Action) =
        all_choices <- Set.add (card, action) all_choices

type AI(board: Board) =
    member val choices = new Choices() with get, set

    member x.Reset() = x.choices <- new Choices()

    member x.getAllHandChoices(player: Player) =
        for card in player.hand.cards do
            x.choices.Add card Action.Play

    member x.getAllDogmaChoices (player: Player) (choice_count: int32) =
        let mutable count = choice_count

        for pile in player.tableau.Stacks do
            x.choices.Add (pile.Value.Top()) Action.Dogma

    member x.IsWinner(player: Player) =
        match board.PlayerCount with
        | 2 -> List.length player.achievements >= 6
        | 3 -> List.length player.achievements >= 5
        | 4 -> List.length player.achievements >= 4
        | otherwise -> failwith $"bad player count {board.PlayerCount}"


    member x.PlayerScore(player: Player) : int =
        let mutable score =
            if x.IsWinner player then
                // Winning is everything, maybe should use infinity here!
                1_000_000
            else
                0

        score <- score + (List.length player.achievements) * 10_000

        let topCard = player.tableau.GetHighestTopCard()
        score <- score + (5 * topCard.age * topCard.age)

        // Award points for values of other top cards
        // Award points for splays ( Up = 40, Right = 25, Left = 10)
        // Award 20 points for each color in play + 1 point for the depth of the pile
        for p in player.tableau.Stacks.Values do
            let card = p.Top()
            score <- score + (List.length p.cards) + p.Top().age
            score <- score + SplayDirection.Score p.splayed

        // Award 75 points for each icon you're winning (50 for ties), plus 2 for each extra icon
        // TODO: Update once we keep track of icons

        // Award points equal to the total value of hand
        // TODO: Update once we keep track of icons
        for card in player.hand.cards do
            score <- score + 2 * card.age

        // Award points for your score pile (only do exponents for scores that are close to your top card)
        score <- score + player.hand.Score()

        score
